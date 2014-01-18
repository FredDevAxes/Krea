--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description Camera is a module allowing to manipulate the camera view during the application.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("camera", package.seeall)

--- 
-- Members of the Object Instance
-- @class table
-- @name Fields
-- @field name The name of the camera instance

Camera = {}
Camera.__index = Camera



--- 
-- @description Create a new Camera Instance
-- @return Return a new Camera Instance
-- @param params Lua Table containing the params
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <li> local curveInstance = require("object").Object.create(params)</code></li>
-- </ul>	
function Camera.create()
	
	--Init attributes of instance
	local camera = {}           
	setmetatable(camera,Camera)

	camera.currentGroups = nil
	
	camera.isDragEnabled = false
	camera.isAutoFocusEnabled = false
	
	camera.currentObjectToFollow = nil
	
	camera.lastDragX = nil
	camera.lastDragY = nil
	
	camera.LastPosX_Focus = nil
	camera.LastPosY_Focus = nil
	return camera
end

function Camera:setDragEnabled(displayGroupsToMove)
	self:stopAutoFocus()
	self:setDragDisable()
	
	self.currentGroups = displayGroupsToMove
	
	if(self.currentGroups) then 
		self.onDrag = function(event)
			local phase = event.phase
			if(phase == "began") then 
				self.lastDragX = event.x 
				self.lastDragY = event.y
			elseif(phase == "moved" or phase == "ended") then 
				if(self.lastDragX and self.lastDragY) then 
					local xMove = event.x - self.lastDragX
					local yMove = event.y - self.lastDragY
					for i = 1,#self.currentGroups do 
						local group = self.currentGroups[i]
						group.x = group.x + xMove
						group.y = group.y + yMove
					end
				end
				
				self.lastDragX = event.x 
				self.lastDragY = event.y
			end
			
			return true
		end
		
		Runtime:addEventListener("touch",self.onDrag)
		self.isDragEnabled = true
	end

end

function Camera:setDragDisable()
	if(self.onDrag) then 
		Runtime:removeEventListener("touch",self.onDrag)
		self.onDrag = nil
	end
	self.isDragEnabled = false
end

function Camera:startAutoFocusOnObject(objectInstance)
	self:stopAutoFocus()
	self:setDragDisable()
	
	if(objectInstance) then 
		self.onObjectMove = function()
			local layerParent = objectInstance.displayGroupParent
			local objX, objY =  objectInstance.object:localToContent(0,0)
			if(objX <= self.scrollWindowLeft or objY <= self.scrollWindowTop or objX >= self.scrollWindowRight or objY >= self.scrollWindowBottom) then
				local currentX = objectInstance.object.x * layerParent.xScale
				if(self.LastPosX_Focus ~= nil) then
					layerParent.x = layerParent.x - (currentX - self.LastPosX_Focus)
				end
				self.LastPosX_Focus = currentX 
			else
				self.LastPosX_Focus = nil 
			end
			
			if(objY <= self.scrollWindowTop  or objY >= self.scrollWindowBottom) then
				local currentY = objectInstance.object.y * layerParent.xScale
				if(self.LastPosY_Focus ~= nil) then
					layerParent.y = layerParent.y - (currentY - self.LastPosY_Focus)
				end
				self.LastPosY_Focus = currentY
			else
				self.LastPosY_Focus = nil 
			end
			
		end
		
		Runtime:addEventListener("enterFrame",self.onObjectMove)
		
	end
end

function Camera:stopAutoFocus()
	if(self.onObjectMove) then 
		Runtime:removeEventListener("enterFrame",self.onObjectMove)
		self.onObjectMove = nil
	end
	self.isAutoFocusEnabled = false
end
















