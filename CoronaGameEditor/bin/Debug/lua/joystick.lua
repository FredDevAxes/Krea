-- @author Matthew Pringle
-- @copyright The software is provided FREE to use and is on an "AS IS" basis.
-- @release 1.0 
-- @description Joystick is a lua class providing a generic joystick control.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

-- @usage Usage to get joystick values in the onJoy_Move function:
-- <ul>
-- <li><code>onJoystick__Move = function(event)
-- <br>		local joyX = event.joyX -- ( -1 to 1 )
-- <br>		local joyY = event.joyY -- ( -1 to 1 )
-- <br>		local joyAngle = event.joyAngle -- ( returns full 360 degrees, 0 pointing up and degrees increasing clockwise )
-- <br>		local joyVector = event.joyVector -- ( returns 0 to 1 )
-- <br>		--Check each values for false
-- <br>		if(joyX and joyY and joyAngle and joyVector) then 
-- <br>			-- DO SOMETHING
-- <br>		end
-- <br>end
-- <br>-- To start the joystick call joystick.joystickStart() (KREA WILL AUTOMATICALLY START A JOYSTICK AT ENTER SCENE)
-- <br>-- To stop the joystick from interacting with the user call joystick.joystickStop() (KREA WILL AUTOMATICALLY STOP A JOYSTICK AT EXIT SCENE)
-- </ul>	

------------------------------------------------------------------------------------------------------------------------
module(..., package.seeall)
------------------------------------------------------------------------------------------------------------------------
-- Calculate Joytick Outputs
local function calculateJoystick( this )
	
	-- Setup Values
	local joystickTouch = this.joystickTouch
	local newXPosition = this.joyCentreX
	local newYPosition = this.joyCentreY
	local max = this.joyMax
	local pi = math.pi
	local sqrt = math.sqrt
	local atan2 = math.atan2
	local sin = math.sin
	local cos = math.cos
	
	-- Calculate X and Y
	local xDist = joystickTouch.x - newXPosition
	local yDist = joystickTouch.y - newYPosition
	
	-- Calcualte Distance
	local distance = sqrt ( ( xDist * xDist ) + ( yDist * yDist ) )
	
	-- Set Max Distance
	if distance > max then distance = max end
	
	-- Calculate Angle
	local angle = ( atan2( yDist , xDist ) * ( 180 / pi ) ) + 90
	if angle < 0 then angle = 360 + angle end
	
	-- Calculate X and Y Limited To Outer Circle
	local xFinal = ( sin( angle * pi / 180 ) * distance )
	local yFinal = ( cos( angle * pi / 180 ) * distance )
	
	-- Normalize
	distance = distance / max
	xFinal = xFinal / max
	yFinal = yFinal / max
	
	-- Set New Values
	this.joyVector = distance
	this.joyAngle = angle
	this.joyX = xFinal
	this.joyY = -yFinal
	
end
------------------------------------------------------------------------------------------------------------------------
-- Position Inner Joystick
local function positionJoystickInner( this )
	
	-- Setup Values
	local newXPosition = this.joyCentreX
	local newYPosition = this.joyCentreY
	local max = this.joyMax
	local joyX = this.joyX
	local joyY = this.joyY
	local floor = math.floor
	
	-- Set New Position
	if joyX ~= false and joyY ~= false then
		this.joystickInner.x = floor( newXPosition + ( joyX * max ) )
		this.joystickInner.y = floor( newYPosition + ( joyY * max ) )
	else
		-- Reset Position
		this.joystickInner.x = floor( newXPosition )
		this.joystickInner.y = floor( newYPosition )
	end
	
end
------------------------------------------------------------------------------------------------------------------------
-- Handle Joystick Touch
local function newJoystickHandler( self , event )
	
	-- Setup Values
	local joystick = self.parent
	local this = event.target
	local phase = event.phase
	local onMove = joystick.onMove
	
	-- Event
	if "began" == phase then
		-- Start Focus
		display.getCurrentStage():setFocus( this , event.id )
		this.isFocus = true
		-- Store Initial Position
		this.x0 = event.x - this.x
		this.y0 = event.y - this.y
		-- Show Ghost
		joystick.joystickTouch.alpha = 1
		-- Fade Effect
		if joystick.fadeEffect == true then
			if joystick.tween then transition.cancel( joystick.tween ) end
			local tweenTime = ( 1 - joystick.alpha ) * 300
			joystick.tween = transition.to( joystick, { alpha=1, time=tweenTime, delay=0 } )
		end
	elseif this.isFocus then
		if "moved" == phase then
			-- Capture Event Position
			this.x = event.x - this.x0
			this.y = event.y - this.y0
			-- Perform Calculations
			calculateJoystick( joystick )
			-- Set Position Of Inner Joystick
			positionJoystickInner( joystick )
		elseif "ended" == phase or "cancelled" == phase then
			-- End Focus
			display.getCurrentStage():setFocus( this , nil )
			this.isFocus = false
			-- Reset Position To Joystick Centre
			this.x = joystick.joyCentreX
			this.y = joystick.joyCentreY
			-- Set Values To False
			joystick.joyVector = false
			joystick.joyAngle = false
			joystick.joyX = false
			joystick.joyY = false
			-- Set Position Of Inner Joystick
			positionJoystickInner( joystick )
			-- Hide Ghost
			joystick.joystickTouch.alpha = 0.01
			-- Fade Effect
			if joystick.fadeEffect == true then
				local tweenTime = ( 1 - joystick.fadeEffectAlpha ) * 500
				if joystick.tween then transition.cancel( joystick.tween ) end
				if joystick.alpha < 1 then
					joystick.tween = transition.to( joystick , { alpha=joystick.fadeEffectAlpha , time=tweenTime , delay=0 } )
				else
					joystick.tween = transition.to( joystick , { alpha=joystick.fadeEffectAlpha , time=tweenTime , delay=joystick.fadeEffectDelay } )
				end
			end
		end
	end
	-- End Event
	return true
	
end
------------------------------------------------------------------------------------------------------------------------
-- Generates A New Joystick
function newJoystick( params )

	-- New Joystick Group
	local joystick = display.newGroup()
	
	-- Load Background Image
	if params.backgroundImage and params.backgroundImage ~= "" then
		local joystickBackground = display.newImage( params.backgroundImage )
		if not params.background_x then params.background_x = 0 end
		joystickBackground.x =  params.background_x
		if not params.background_y then params.background_y = 0 end
		joystickBackground.y =  params.background_y
		joystick:insert( joystickBackground )
		joystick.joystickBackground = joystickBackground
		if not params.backgroundAlpha then params.backgroundAlpha = 1 end
		joystick.joystickBackground.alpha = params.backgroundAlpha

	end
	
	-- Load Outer Image
	if not params.outerRadius or params.outerRadius == "" then params.outerRadius = 60 end
	if params.outerImage and params.outerImage ~= "" then
		local joystickOuter = display.newImage( params.outerImage )
		joystick:insert( joystickOuter )
		joystick.joystickOuter = joystickOuter
	else
		-- Or Draw Outer Circle
		local joystickOuter = display.newCircle( params.outerRadius , params.outerRadius , params.outerRadius )
		joystickOuter:setFillColor( 255 , 255 , 255 )
		joystick:insert( joystickOuter )
		joystick.joystickOuter = joystickOuter
	end
	if not params.outerAlpha or params.outerAlpha == "" then params.outerAlpha = 1 end
	joystick.joystickOuter.alpha = params.outerAlpha
	
	-- Load Inner Image
	if not params.innerRadius or params.innerRadius == "" then params.innerRadius = 24 end
	if params.innerImage and params.innerImage ~= "" then
		local joystickInner = display.newImage( params.innerImage )
		joystick:insert( joystickInner )
		joystick.joystickInner = joystickInner
	else
		-- Or Draw Inner Circle
		local joystickInner = display.newCircle( params.outerRadius , params.outerRadius , params.innerRadius )
		joystickInner:setFillColor( 0 , 0 , 0 )
		joystick:insert( joystickInner )
		joystick.joystickInner = joystickInner
	end
	if not params.innerAlpha or params.innerAlpha == "" then params.innerAlpha = 1 end
	joystick.joystickInner.alpha = params.innerAlpha
	
	-- Generate Touch Object
	if not params.ghost or params.ghost == "" then params.ghost = 0 end
	joystickTouch = display.newCircle( joystick.joystickOuter.x , joystick.joystickOuter.y , joystick.joystickInner.width / 2 )
	joystickTouch:setFillColor( 255 , 255 , 255 , params.ghost )
	joystickTouch.alpha = 0.01
	joystick:insert( joystickTouch )
	joystick.joystickTouch = joystickTouch
	
	-- Position Joystick
	if not params.position_x or params.position_x == "" then params.position_x = 15 end
	if not params.position_y or params.position_y == "" then params.position_y = display.stageHeight - 15 - joystick.joystickOuter.height end
	joystick.x = params.position_x
	joystick.y = params.position_y
	
	
	
	-- Set Joystick Alpha
	if not params.joystickAlpha or params.joystickAlpha == "" then params.joystickAlpha = 1 end
	joystick.alpha = params.joystickAlpha
	
	-- Fade Effect
	if params.joystickFade == true then 
		joystick.fadeEffect = true
		joystick.fadeEffectAlpha = joystick.alpha
		if not params.joystickFadeDelay or params.joystickFadeDelay == "" then
			params.joystickFadeDelay = 2000
		end
		joystick.fadeEffectDelay = params.joystickFadeDelay
	end
	
	-- Set Default Values
	joystick.joyMax = ( joystick.joystickOuter.width / 2 ) - ( joystick.joystickInner.width / 2 )
	joystick.joyCentreX = joystick.joystickOuter.x
	joystick.joyCentreY = joystick.joystickOuter.y
	joystick.joyVector = false
	joystick.joyAngle = false
	joystick.joyX = false
	joystick.joyY = false
	
	-- On Move Event
	if ( params.onMove and ( type(params.onMove) == "function" ) ) then
		joystick.enterFrame = params.onMove
		Runtime:addEventListener( "enterFrame" , joystick )
	end

	-- Set Joystick Listener
	joystick.joystickTouch.touch = newJoystickHandler
	joystick.joystickTouch:addEventListener( "touch", joystick.joystickTouch )
	
	-- Stop Function
	function joystick:joystickStop()
		-- End Focus
		display.getCurrentStage():setFocus( joystick.joystickTouch , nil )
		joystick.joystickTouch.isFocus = false
		-- Reset Position To Joystick Centre
		joystick.joystickTouch.x = joystick.joyCentreX
		joystick.joystickTouch.y = joystick.joyCentreY
		joystick.joystickTouch.alpha = 0.01
		joystick.joystickTouch:removeEventListener( "touch", joystick.joystickTouch )
		joystick.joyVector = false
		joystick.joyAngle = false
		joystick.joyX = false
		joystick.joyY = false
		if joystick.enterFrame then
			Runtime:removeEventListener( "enterFrame" , joystick )
			joystick.enterFrame( joystick )
		end
		positionJoystickInner( joystick )
		-- Fade Effect
		if joystick.fadeEffect == true then 
			if joystick.alpha ~= joystick.fadeEffectAlpha then
				local tweenTime = ( joystick.alpha - joystick.fadeEffectAlpha ) * 500
				if joystick.tween then transition.cancel( joystick.tween ) end
				joystick.tween = transition.to( joystick , { alpha=joystick.fadeEffectAlpha , time=tweenTime } )
			end
		end
	end
	
	-- Start Function
	function joystick:joystickStart()
		joystick.joyVector = false
		joystick.joyAngle = false
		joystick.joyX = false
		joystick.joyY = false
		if joystick.enterFrame then
			Runtime:addEventListener( "enterFrame" , joystick )
		end
		joystick.joystickTouch:addEventListener( "touch", joystick.joystickTouch )
	end
	
	-- Position Joystick Inner
	positionJoystickInner( joystick )
	
	-- Return New Joystick
	return joystick
	
end