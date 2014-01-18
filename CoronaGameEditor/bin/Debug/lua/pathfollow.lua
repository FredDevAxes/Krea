--- This module is provided  as it by Native-Software for use into a Krea project.
-- @description Module managing the path that an object should follow
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>
-- @copyright Native-Software 2012- All Rights Reserved.
-- @author Frederic Raimondi
-- @release 1.0 
-- @see object


module("pathfollow", package.seeall)


--- 
-- Members of the Path follow Instance
-- @class table
-- @name Fields
-- @field params The table params used to create the path follow instance
-- @field targetObject The Object instance to move
-- @field iteration The number of transition iteration
-- @field rotate Indicates whether the path follow make the Corona display object rotating during the transition
-- @field speed The time in milliseconds for each transition cycle
-- @field path The table containing all points to reach
	
	
PathFollow = {}
PathFollow.__index = PathFollow

local pi = math.pi
local atan2 = math.atan2
local sqrt = math.sqrt



--- 
-- @description Create a new Instance of path follow for a object
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.targetObject = obj
-- <br>params.path = {x1,y1,x2,y2,x3,y3,...}
-- <br>params.isCyclic = true 
-- <br>params.speed = 5000
-- <br>params.iteration = 10 
-- <br>params.removeOnComplete = true</li>
-- <li>local pathF = require("pathfollow").PathFollow.create(params)</code> </li>
-- </ul></br>
-- @param params Lua Table containing the params
-- @return Instance of PathFollow object
function PathFollow.create(params)
	
	--Init attributes of instance
	local pathFollow = {}           
	setmetatable(pathFollow,PathFollow)
	
	--Init attributes
	pathFollow.params = params
	pathFollow.targetObject = params.targetObject
	pathFollow.iteration = params.iteration
	pathFollow.rotate = params.rotate
	pathFollow.removeOnComplete = params.removeOnComplete
	
	if(pathFollow.iteration <= 0) then pathFollow.iteration = -1 end
	pathFollow.currentIteration = 1
	
	pathFollow.speed = params.speed
	pathFollow.path = params.path
	pathFollow.pointsByStep = {}
	
	local totalPoints = 0
	pathFollow.timeByPoint = 0
	

	local pow = math.pow
	local path = pathFollow.path
	
	for i = 1,#path -1 ,1 do 
		
		local currentX = path[i].x
		local currentY = path[i].y
		local nextX = path[i+1].x
		local nextY = path[i+1].y
		local nbPoints = sqrt(pow(nextY - currentY,2) + pow(nextX - currentX,2))
		table.insert(pathFollow.pointsByStep,nbPoints)
		
		totalPoints = totalPoints + nbPoints

	end
	
	pathFollow.timeByPoint = pathFollow.speed / totalPoints 
	pathFollow.isCyclic = params.isCyclic

	pathFollow.isStarted = false
	pathFollow.currentIndexPath = 2
	pathFollow.currentDirection = "NORMAL" -- or "REVERSE"

	pathFollow.onCompleteTransition = function()
		pathFollow:goNextPoint()

	end
	
	return pathFollow
end

--- 
-- @description Start the object transition following the path
-- @usage Usage:
-- <ul>
-- <li><code>pathF:start()</code></li>
-- </ul></br>
function PathFollow:start()
	if(self.isStarted == false ) then 
		self.isStarted = true
		self:goNextPoint()
	end
end

--- 
-- @description Pause the object transition following the path
-- @usage Usage:
-- <ul>
-- <li><code>pathF:pause()</code></li>
-- </ul></br>
function PathFollow:pause()
	if(self.isStarted == true ) then 

		if(self.currentTransition) then 
			transition.cancel(self.currentTransition)
		end
		self.isStarted = false
		
	end
end

--- 
-- @description Go to the next point in the transition throught the path
-- @usage Usage:
-- <ul>
-- <li><code>pathF:goNextPoint()</code></li>
-- </ul></br>
function PathFollow:goNextPoint()

	local path = self.path
	local currentIndexPath = self.currentIndexPath
	local obj = self.targetObject.object
	
	if(path) then 
		if(#path >= currentIndexPath and currentIndexPath >=1) then 
			local nextX = path[currentIndexPath].x
			local nextY = path[currentIndexPath].y
			local nbPoints = nil
			if(self.currentDirection == "NORMAL") then 
				nbPoints = self.pointsByStep[currentIndexPath -1]
			elseif(self.currentDirection == "REVERSE") then 
				nbPoints = self.pointsByStep[self.currentIndexPath]
			end
					
			local timeByStep = nbPoints * self.timeByPoint
			if(self.currentDirection == "NORMAL") then
				self.currentIndexPath  = self.currentIndexPath +1
			elseif(self.currentDirection == "REVERSE") then
				self.currentIndexPath  = self.currentIndexPath -1
			end

			
			if(self.rotate == true) then

				local x1 = obj.x
				local y1 = obj.y

					
				local angle = atan2 (nextY - y1, nextX - x1) * 180 / pi
				local diff = angle - obj.rotation
				if (diff < -180) then diff = diff +360 end
				if (diff > 180) then diff = diff - 360 end
				angle = obj.rotation +diff

				self.currentTransition = transition.to(obj,{time = timeByStep,rotation = angle,x = nextX, y = nextY,onComplete = self.onCompleteTransition})
			else
				self.currentTransition = transition.to(obj,{time = timeByStep,x = nextX, y = nextY,onComplete = self.onCompleteTransition})
			end
			
			
		else
			if(self.currentIteration < self.iteration  or self.iteration == -1) then 
				if(self.isCyclic == true ) then 
					if(self.currentDirection == "NORMAL") then 
						self.currentDirection = "REVERSE"
						self.currentIndexPath = #self.path-1
					elseif(self.currentDirection == "REVERSE") then 
						self.currentDirection ="NORMAL"
						self.currentIndexPath = 2
						obj.rotation = self.targetObject.params.rotation
					end
				else
					self.currentIndexPath = 2
					obj:setReferencePoint(display.TopLeftReferencePoint)
					obj.x = self.targetObject.xOrigin
					obj.y = self.targetObject.yOrigin
					obj:setReferencePoint(display.CenterReferencePoint)
				end
				
				self.currentIteration = self.currentIteration+1
				self:goNextPoint()
			else
				if(self.removeOnComplete == true) then
					obj:removeSelf()
				end
			end
			
		end
	end
	
end

--- 
-- @description Clean the PathFollow instance
-- @usage Usage:
-- <ul>
-- <li><code>pathF:clean()</code></li>
-- <li><code>pathF = nil</code></li>
-- </ul></br>
function PathFollow:clean()
	self:pause()
	self.targetObject = nil
	self.path = nil
	self.speed = nil

end