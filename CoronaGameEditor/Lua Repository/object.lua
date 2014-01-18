--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description Object is a generic class to create a corona display object.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("object", package.seeall)

--- 
-- Members of the Object Instance
-- @class table
-- @name Fields
-- @field name The name of the object instance
-- @field object The Corona display object of the object instance
-- @field displayGroupParent The display group where is stored the display object
-- @field params The table params used to create the object instance
-- @field type The display object type of the object instance
-- @field xOrigin The x origin location of the object instance
-- @field yOrigin The y origin location of the object instance
-- @field hasBody Boolean that indicates whether the display object has a physics body or not
Object = {}
Object.__index = Object



--- 
-- @description Create a new Object Instance
-- @return Return a new Object Instance
-- @param params Lua Table containing the params
-- @usage Usage to create a Rectangle:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "RECTANGLE"
-- <br>params.isRounded = true
-- <br>params.cornerRadius = 5
-- <br>params.x = -50
-- <br>params.y = background.contentWidth *(0.5)
-- <br>params.width = 100
-- <br>params.height = 10
-- <br>params.displayGroupParent = layer1
-- <br>params.alpha = 0.1
-- <br>params.backColor = { R = 0,G = 0,B = 250}
-- <br>params.strokeColor = { R = 0,G = 0,B = 250}
-- <br>params.strokeWidth = 2
-- <li> local rectangleInstance = require("object").Object.create(params)</code></li>
-- </ul>		
-- <br>Usage to create a Circle:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "CIRCLE"
-- <br>params.x = -50
-- <br>params.y = background.contentWidth *(0.5)
-- <br>params.radius = 100
-- <br>params.displayGroupParent = layer1
-- <br>params.alpha = 0.1
-- <br>params.backColor = { R = 0,G = 0,B = 250}
-- <br>params.strokeColor = { R = 0,G = 0,B = 250}
-- <br>params.strokeWidth = 2
-- <li> local circleInstance = require("object").Object.create(params)</code></li>
-- </ul>	
-- <br>Usage to create a Line:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "LINE"
-- <br>params.x1 = -50
-- <br>params.y1 = 100
-- <br>params.x2 = -200
-- <br>params.y2 = 100
-- <br>params.displayGroupParent = layer1
-- <br>params.alpha = 0.1
-- <br>params.backColor = { R = 0,G = 0,B = 250}
-- <br>params.strokeWidth = 2
-- <li> local lineInstance = require("object").Object.create(params)</code></li>
-- </ul>
-- <br>Usage to create an Image:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "IMAGE"
-- <br>params.x = -50
-- <br>params.y = background.contentWidth *(0.5)
-- <br>params.width = 100
-- <br>params.height = 10
-- <br>params.displayGroupParent = layer1
-- <br>params.alpha = 0.1
-- <br>params.pathImage = "objet.png"
-- <li> local imageInstance = require("object").Object.create(params)</code></li>
-- </ul>
-- <br>Usage to create a Sprite:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "SPRITE"
-- <br>params.x = -50
-- <br>params.y = background.contentWidth *(0.5)
-- <br>params.spriteSet = spriteSet
-- <br>params.displayGroupParent = layer1
-- <br>params.alpha = 0.1
-- <li> local spriteInstance = require("object").Object.create(params)</code></li>
-- </ul>
-- <br>Usage to create a Curve:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "CURVE"
-- <br>params.tabPoints = {x1,y1,x2,y2,...}
-- <br>params.displayGroupParent = layer1
-- <br>params.alpha = 0.1
-- <br>params.strokeColor = { R = 0,G = 0,B = 250}
-- <br>params.strokeWidth = 2
-- <li> local curveInstance = require("object").Object.create(params)</code></li>
-- </ul>	
-- <br>Usage to create a Text:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.type = "TEXT"
-- <br>params.x = -50
-- <br>params.y = background.contentWidth *(0.5)
-- <br>params.displayGroupParent = layer1
-- <br>params.text = "text"
-- <br>params.alpha = 0.1
-- <br>params.textColor = { R = 0,G = 0,B = 250}
-- <br>params.textSize = 2
-- <li> local curveInstance = require("object").Object.create(params)</code></li>
-- </ul>	
function Object.create(params)
	
	--Init attributes of instance
	local object = {}           
	setmetatable(object,Object)
	

	--Init attributes
	
	object.name = params.name
	object.displayGroupParent = params.displayGroupParent
	object.params = params
	object.type = params.type
	object.isStarted = false
	object.listEvents = {}
	object.isHandledByTilesMap = params.isHandledByTilesMap
	object.entityParent = params.entityParent
	object.autoPlay = params.autoPlay
	
	local xDest = params.x
	local yDest =params.y

	--Check if pathImage initalized
	if(params.type == "IMAGE") then 
		--Create  obj from an image
		object.object = display.newImageRect(params.pathImage,params.width,
												params.height)
		object.object:setReferencePoint(display.TopLeftReferencePoint)
		object.object.x = xDest
		object.object.y = yDest
		
	elseif(params.type == "SPRITE") then
		-- Create a Sprite From spriteSet
		object.object = sprite.newSprite(params.spriteSet)
		
		object.object:setReferencePoint(display.TopLeftReferencePoint)
	
		object.object.x = xDest - (object.object.contentWidth/2) *(1-params.scale)
		object.object.y = yDest - (object.object.contentHeight/2) *(1 -params.scale)
		if(params.scale) then 
			object.object:scale(params.scale,params.scale)
		end
		object.currentSequence = params.sequenceName
		object.firstFrameIndex = params.firstFrameIndex
		
		object.object:prepare(object.currentSequence)
				
		if(object.firstFrameIndex) then 
			object.object.currentFrame = object.firstFrameIndex
		end
		
	elseif(params.type == "RECTANGLE") then
		if(params.isRounded == true) then 
			--Create a rounded rect with expected colors
			object.object = display.newRoundedRect(params.x,params.y,params.width,
													params.height,params.cornerRadius)
		else
			--Create a rect with expected colors
			object.object = display.newRect(params.x,params.y,params.width,
													params.height)
		end
		
		if(params.backColor) then 
			object.object:setFillColor(params.backColor.R,params.backColor.G,params.backColor.B)
		end
		
		if(params.strokeColor) then 
			object.object:setStrokeColor(params.strokeColor.R,params.strokeColor.G,params.strokeColor.B)
		end
		
		if(params.strokeWidth) then 
			object.object.strokeWidth = params.strokeWidth
		end
		
		if(params.gradient) then 
			local g = graphics.newGradient(params.gradient[1],params.gradient[2],params.gradient[3])
			object.object:setFillColor(g)
		end
		object.object:setReferencePoint(display.TopLeftReferencePoint)
		object.object.x = xDest
		object.object.y = yDest
		
		
	elseif(params.type == "CIRCLE") then
		--Create a Circle with expected colors
		object.object = display.newCircle(params.x + params.radius
							,params.y +params.radius
							,params.radius)	
		
		if(params.backColor) then 
			object.object:setFillColor(params.backColor.R,params.backColor.G,params.backColor.B)
			
		end
		
		if(params.strokeColor) then 
			object.object:setStrokeColor(params.strokeColor.R,params.strokeColor.G,params.strokeColor.B)
		end
		
		if(params.strokeWidth) then 
			object.object.strokeWidth = params.strokeWidth
		end
		
	elseif(params.type == "CURVE" or params.type == "LINE") then
		if(#params.tabPoints >3) then 
			--Create a first line 
			object.object = display.newLine(params.tabPoints[1],
										params.tabPoints[2],
										params.tabPoints[3],
										params.tabPoints[4])
										
			object.object:setReferencePoint(display.TopLeftReferencePoint)
			
			for i = 5, #params.tabPoints, 2 do 
				object.object:append(params.tabPoints[i],params.tabPoints[i+1])
			end
			
			if(params.backColor) then 
				object.object:setColor(params.backColor.R,params.backColor.G,params.backColor.B)
			end
			
			if(params.strokeWidth) then 
				object.object.width = params.strokeWidth
			end
			
			object.object:setReferencePoint(display.TopLeftReferencePoint)
		end
		
	elseif(params.type == "TEXT") then
		local finalText = rosetta:getString(params.text)
		if(finalText == nil or finalText =="NO STRING") then finalText = params.text end
		
		if(params.textFont and params.textFont ~= "DEFAULT") then 
		
			local onSimulator = system.getInfo( "environment" ) == "simulator" or system.getInfo("platformName") == "Android"
			local platformVersion = system.getInfo( "platformVersion" )
			local olderVersion = tonumber(string.sub( platformVersion, 1, 1 )) < 4

			local fontName = params.textFont
			local fontSize = params.textSize

			-- if on older device (and not on simulator) ...
			if not onSimulator and olderVersion then
				if string.sub( platformVersion, 1, 3 ) ~= "3.2" then
				fontName = native.systemFont
				fontSize = 22
				end

			end
			
			object.object = display.newText(finalText,params.x,params.y,params.width,params.height,fontName,fontSize)
		else
			object.object = display.newText(finalText,params.x,params.y,params.width,params.height,native.systemFont,params.textSize)
		end
		
		
		object.text = finalText
		if(params.textColor) then 
			object.object:setTextColor(params.textColor.R, params.textColor.G, params.textColor.B)
		end
		
		if(params.gradient) then 
			local g = graphics.newGradient(params.gradient[1],params.gradient[2],params.gradient[3])
			object.object:setTextColor(g)
		end
		
		object.object:setReferencePoint(display.TopLeftReferencePoint)
		object.object.x = xDest
		object.object.y = yDest
	end
	
	if(params.alpha) then 
		object.object.alpha = params.alpha
	end
	
	if(params.blendMode) then 
		object.object.blendMode = params.blendMode
	end
	
	if(params.indexInGroup) then 
		object.displayGroupParent:insert(params.indexInGroup,object.object)
	else
		object.displayGroupParent:insert(object.object)
	end
	
	
	object.object.getInstance = function()
		return object
	end
	object.object:setReferencePoint(display.CenterReferencePoint)
	object.xOrigin = object.object.x
	object.yOrigin = object.object.y
	
	if(params.rotation) then 
		object.object.rotation = params.rotation
	end
	
	object.isDraggable = params.isDraggable
	object.dragMaxForce = params.dragMaxForce
	object.dragDamping = params.dragDamping
	object.dragFrequency = params.dragFrequency
	
	object.dragBody = params.dragBody

	object.isLocked = false
	object.hasBody = false
	return object
end

--- 
-- @description Clone the instance of this object
-- @return Object The new clone of the object
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:clone()</code></li>
-- </ul>
function Object:clone(insertCloneAtEndOfGroup)
	
	if(insertCloneAtEndOfGroup) then 
		self.params.indexInGroup = nil
	else
		local index = self:getIndexInGroup()
		self.params.indexInGroup = index +1
	end
	
	
	local obj = Object.create(self.params)
	if(self.hasBody == true) then 
		local body = require("body"..string.lower(obj.name)).startBody(obj)
	end
	
	obj.currentSequence = self.currentSequence
	if(self.pathFollow) then 
		local params = self.pathFollow.params
		params.targetObject = obj
		obj.pathFollow = require("pathfollow").PathFollow.create(params)
	end
	return obj
	
end

function Object:getIndexInGroup()
	if(self.displayGroupParent) then 
		for i = 1,self.displayGroupParent.numChildren do 
			local child = self.displayGroupParent[i]
			if(child == self.object) then 
				return i
			end
		end
	end
	
	return 1
end

--- 
-- @description Clean the current object Instance
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:clean()</li>
-- <li>objectInstance = nil</code></li>
-- </ul>
function Object:clean()
	if(not self.isDestroyed) then
		self.isDestroyed = true
		if(self.onDelete) then 
			self.onDelete()
		end
		
		if(self.pathFollow) then 
			self.pathFollow:pause()
			self.pathFollow = nil
		end
		--Init attributes
		self.name = nil
		self.displayGroupParent = nil
		self.params = nil
		self.isStarted = nil
		self.listEvents = nil

		if(self.object.removeSelf) then 
			self.object:removeSelf()
		end
	end
end


function Object:createEventListener(event)
	if(self.object) then 
		table.insert(self.listEvents,event)
		self.object:addEventListener(event.eventType,event.handle)
	end
end

function Object:removeEventListener(eventName)
	if(self.object) then 
		for i = 1,#self.listEvents do 
			local child = self.listEvents[i]
			if(child) then 
				if(child.eventName == eventName) then 
					table.remove(self.listEvents,i)
					self.object:removeEventListener(child.eventType,child.handle)
					child = nil
					return
				end
			end
		end

	end
end

function Object:createPhysics(mode,params)
	if(self.object) then
		if(params) then 
			physics.addBody(self.object,mode,params)
		else
			physics.addBody(self.object,mode)
		end
	end
end

--- 
-- @description Set the object behaviour at start Interaction
-- @param handle Handle function
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:setOnStartBehaviour(handle)</code></li>
-- </ul>
function Object:setOnStartBehaviour(handle)
	self.onStart = handle
end

--- 
-- @description Set the object behaviour at pause Interaction
-- @param handle Handle function
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:setOnPauseBehaviour(handle)</code></li>
-- </ul>
function Object:setOnPauseBehaviour(handle)
	self.onPause = handle
end

--- 
-- @description Set the object behaviour at clean
-- @param handle Handle function
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:setOnDeleteBehaviour(handle)</code></li>
-- </ul>
function Object:setOnDeleteBehaviour(handle)
	self.onDelete = handle
end

--- 
-- @description Start the interactions of the object
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:startInteractions()</code></li>
-- </ul>
function Object:startInteractions()
	
	if(self.isLocked == false) then 
		if(self.object.removeSelf) then 
			self.object.isBodyActive = true
		end
		
		if(self.isStarted == false) then 
			if(self.onStart) then self.onStart()end
			self.isStarted = true
			
			if(self.isDraggable == true and self.dragBody) then 
				
				self.object:addEventListener("touch",self.dragBody)
			end
			
			if(self.type == "SPRITE") then 

				if(self.autoPlay == true) then 
					self.object:play()
				end
			
			end
			
			if(self.pathFollow) then 
				self.pathFollow:start()
			end
			
			
			
		end
	
		self:updateDisplay()
	
	end
		
end

--- 
-- @description Update the display of the current display object. Example: To update the text content of a text object once the current language has been changed.
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:updateDisplay()</code></li>
-- </ul>
function Object:updateDisplay()
	--Actualise the text of the object
	if(self.type == "TEXT") then
		local finalText = rosetta:getString(self.object.text)
		if(finalText == nil or finalText =="NO STRING") then finalText = self.object.text end
		self.object.text = finalText
	end	
end

--- 
-- @description Pause the interactions of the object
-- @usage Usage:
-- <ul>
-- <li><code>objectInstance:pauseInteractions()</code></li>
-- </ul>
function Object:pauseInteractions()
	
	if(self.object.removeSelf) then 
		self.object.isBodyActive = false
	end
	
	
	if(self.isStarted == true) then 
		if(self.onPause) then self.onPause() end
		self.isStarted = false
		
		if(self.isDraggable == true and self.dragBody) then 
			self.object:removeEventListener("touch",self.dragBody)
		end
			
		if(self.type == "SPRITE") then 
			if(self.currentSequence) then 
				self.object:pause()
			end
		end
		
		if(self.pathFollow) then 
			self.pathFollow:pause()
		end
	end

	
	
end
