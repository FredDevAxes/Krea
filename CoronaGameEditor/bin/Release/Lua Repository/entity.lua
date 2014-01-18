--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description Entity is a module to create entity with Object Instances (see also object.lua)
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("entity", package.seeall)

--- 
-- Members of the Entity Instance
-- @class table
-- @name Fields
-- @field params The table params used to create the Entity instance
-- @field name The name of the Entity


Entity = {}
Entity.__index = Entity

--- 
-- @description Create a new Entity Instance
-- @return Return a new Entity Instance
-- @param params Lua Table containing the params
-- @see object
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"</li>
-- <li> local entityInstance = require("entity").Entity.create(params)</code></li>
-- </ul>

function Entity.create(params)
	
	--Init attributes of instance
	local entity = {}           
	setmetatable(entity,Entity)
	
	
	entity.name = params.name
	entity.isEntity = true
	entity.tabObjects = {}
	entity.tabJoints = {}
	
	
	
	
	return entity
	
end

function Entity:addObject(obj)

	if(obj) then 
		obj.entityParent = self
		self.tabObjects[obj.name] = obj
	end
	
end

function Entity:removeObject(obj,doClean)

	if(obj) then 
		
		-- Remove joints of this object
		local tabJointsUsed = self:getAllObjectJoints(obj)
		if(tabJointsUsed) then 
			for i = 1,#tabJointsUsed do 
				self:removeJoint(tabJointsUsed[i])
			end
			
			tabJointsUsed = nil
		end
		
		self.tabObjects[obj.name] = nil
		
		if(doClean == true) then 
			obj:clean()
			obj = nil
		end
	end
	
end

function Entity:getAllObjectJoints(obj)

	if(obj) then 
		local tabJointsParam = {}
		for k,v in pairs(self.tabJoints) do 
			if(v) then 
				if(v.obj1 == obj.name or v.obj2 == obj.name)then 
					table.insert(tabJointsParam,v.instance)
				end
			end
		end
		
		return tabJointsParam
	end
	
	return nil
end


function Entity:removeJoint(joint)
	if(joint) then 
		local jointParams = nil
		
		for k,v in pairs(self.tabJoints) do
			if(v) then 
				if(joint == v.instance) then 
					jointParams = v
					break
				end
			end
		end
		
		if(jointParams) then
			self.tabJoints[jointParams.name] = nil
			if(jointParams.instance) then
				if(jointParams.instance.removeSelf) then 
					jointParams.instance:removeSelf()
				end
				jointParams.instance = nil
			end
			
		end
		
		jointParams= nil
	end
end

function Entity:createJoint(paramsJoint)

	if(paramsJoint) then 
		local jointInstance = nil
		
		if(paramsJoint.type == "distance") then 
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			local anchorX1 = paramsJoint.anchorX1 
			local anchorY1 = paramsJoint.anchorY1 
			local anchorX2 = paramsJoint.anchorX2 
			local anchorY2 = paramsJoint.anchorY2 
			if(object1 and object2 and anchorX1 and anchorY1 and anchorX2 and anchorY2) then 
				jointInstance = physics.newJoint( "distance", object1.object, object2.object, anchorX1, anchorY1, anchorX2, anchorY2 )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			end
		elseif(paramsJoint.type == "friction") then 
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			local anchorX = paramsJoint.anchorX1 
			local anchorY = paramsJoint.anchorY1 
			if(object1 and object2 and anchorX and anchorY) then 
				jointInstance = physics.newJoint( "friction", object1.object, object2.object, anchorX, anchorY )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			end
		elseif(paramsJoint.type == "piston") then 
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			local anchorX1 = paramsJoint.anchorX1 
			local anchorY1 = paramsJoint.anchorY1 
			local axisDistanceX = paramsJoint.axisDistanceX 
			local axisDistanceY = paramsJoint.axisDistanceY 
			if(object1 and object2 and anchorX1 and anchorY1 and axisDistanceX and axisDistanceY) then 
				jointInstance = physics.newJoint( "piston", object1.object, object2.object, anchorX1, anchorY1, axisDistanceX, axisDistanceY )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			end
		elseif(paramsJoint.type == "pivot") then 
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			local anchorX1 = paramsJoint.anchorX1 
			local anchorY1 = paramsJoint.anchorY1 
			local anchorX2 = paramsJoint.anchorX2 
			local anchorY2 = paramsJoint.anchorY2 
			if(object1 and object2 and anchorX1 and anchorY1 and anchorX2 and anchorY2) then 
				jointInstance = physics.newJoint( "pivot", object1.object, object2.object, anchorX1, anchorY1,anchorX2, anchorY2 )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			elseif(object1 and object2 and anchorX1 and anchorY1) then 
				jointInstance = physics.newJoint( "pivot", object1.object, object2.object, anchorX1, anchorY1 )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			end
		elseif(paramsJoint.type == "pulley") then 
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			
			local anchorX1 = paramsJoint.anchorX1 
			local anchorY1 = paramsJoint.anchorY1
			local anchorX2 = paramsJoint.anchorX2
			local anchorY2 = paramsJoint.anchorY2 
			local objX1 = paramsJoint.objX1
			local objY1 = paramsJoint.objY1
			local objX2 = paramsJoint.objX2
			local objY2 = paramsJoint.objY2
			local ratio = paramsJoint.ratio

			if(object1 and object2 and anchorX1 and anchorY1 and anchorX2 and anchorY2 and objX1 and objY1 and objX2 and objY2) then 
				jointInstance = physics.newJoint( "pulley", object1.object, object2.object, anchorX1, anchorY1, anchorX2, anchorY2, objX1, objY1, objX2, objY2,1.0)
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			end
		elseif(paramsJoint.type == "weld") then 
			
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			local anchorX = paramsJoint.anchorX1 
			local anchorY = paramsJoint.anchorY1 
			if(object1 and object2 and anchorX and anchorY) then 
				jointInstance = physics.newJoint( "weld", object1.object, object2.object, anchorX, anchorY )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
				
			end
		elseif(paramsJoint.type == "wheel") then 
			local object1 = self.tabObjects[paramsJoint.obj1]
			local object2 = self.tabObjects[paramsJoint.obj2]
			local anchorX1 = paramsJoint.anchorX1 
			local anchorY1 = paramsJoint.anchorY1 
			local axisDistanceX = paramsJoint.axisDistanceX
			local axisDistanceY = paramsJoint.axisDistanceY
			if(object1 and object2 and anchorX1 and anchorY1 and axisDistanceX and axisDistanceY) then 
				jointInstance = physics.newJoint( "wheel", object1.object, object2.object, anchorX1, anchorY1, axisDistanceX, axisDistanceY )
				paramsJoint.instance = jointInstance
				self.tabJoints[paramsJoint.name] = paramsJoint
			end
			
		end
		
		
		-- Set params to the joint
		if(paramsJoint.type == "pivot" or paramsJoint.type == "friction" or paramsJoint.type == "weld") then 
			if(paramsJoint.type ~="weld")then 
				
				if(paramsJoint.type ~= "friction") then 
					if(paramsJoint.isMotorEnabled) then 
						jointInstance.isMotorEnabled = paramsJoint.isMotorEnabled
					end
					
					if(paramsJoint.isLimitEnabled) then 
						jointInstance.isLimitEnabled = paramsJoint.isLimitEnabled
					end
				else
					if(paramsJoint.maxForce) then 
						jointInstance.maxForce = paramsJoint.maxForce
					end
					
					if(paramsJoint.maxTorque) then 
						jointInstance.maxTorque = paramsJoint.maxTorque
					end
				end
				
				if(paramsJoint.isMotorEnabled == true and paramsJoint.type ~="friction")then 
					if(paramsJoint.motorSpeed) then 
						jointInstance.motorSpeed = paramsJoint.motorSpeed
					end
					
					if(paramsJoint.maxMotorTorque) then 
						jointInstance.maxMotorTorque = paramsJoint.maxMotorTorque
					end
				end
				
				if(paramsJoint.isLimitEnabled == true and paramsJoint.type ~="friction")then 
					if(paramsJoint.lowerLimit and paramsJoint.upperLimit) then
						jointInstance:setRotationLimits(paramsJoint.lowerLimit,paramsJoint.upperLimit)
					end
				end
			end
		
		elseif(paramsJoint.type == "distance") then 
		
			if(paramsJoint.frequency) then 
				jointInstance.frequency = paramsJoint.frequency
			end
			
			if(paramsJoint.dampingRatio) then 
				jointInstance.dampingRatio = paramsJoint.dampingRatio
			end
			
		elseif(paramsJoint.type == "piston" or paramsJoint.type == "wheel")then 
			if(paramsJoint.isMotorEnabled) then 
				jointInstance.isMotorEnabled = paramsJoint.isMotorEnabled
			end
			
			if(paramsJoint.isLimitEnabled) then 
				jointInstance.isLimitEnabled = paramsJoint.isLimitEnabled
			end
			
			if(paramsJoint.type == "piston") then 
				if(paramsJoint.isMotorEnabled == true)then 
					if(paramsJoint.motorSpeed) then 
						jointInstance.motorSpeed = paramsJoint.motorSpeed
					end
					
					if(paramsJoint.maxMotorForce) then 
						jointInstance.maxMotorForce = paramsJoint.maxMotorForce
					end
				end
			elseif(paramsJoint.type == "wheel") then 
				if(paramsJoint.isMotorEnabled == true)then 
					if(paramsJoint.motorSpeed) then 
						jointInstance.motorSpeed = paramsJoint.motorSpeed
					end
				end
			end
			
			if(paramsJoint.isLimitEnabled ==true) then 
				if(paramsJoint.lowerLimit and paramsJoint.upperLimit) then
					jointInstance:setLimits(paramsJoint.lowerLimit,paramsJoint.upperLimit)
				end
			end
		end
		
		
		return jointInstance
	end
	
end

function Entity:clone(insertAtEndOfGroup)
	
	local newEntity = Entity.create({name = self.name})
	
	for k,v in pairs(self.tabObjects) do
		if(v) then 
			
			local newObject = v:clone(insertAtEndOfGroup)
			if(newObject) then
				newEntity:addObject(newObject)
				
			end
			
			
		end
	end
	
	for k,v in pairs(self.tabJoints) do
		if(v) then 
			local newJointParam = copyTable(v)
			local joint = newEntity:createJoint(newJointParam)
		end
	end
	
	
	return newEntity
end

function Entity:clean()
	
	for k,v in pairs(self.tabJoints) do

		if(v) then 
			if(v.instance) then 
				if(v.instance.removeSelf) then 
					v.instance:removeSelf()
				end
			end
			v = nil
		end
		
	end
	self.tabJoints = nil
	
	for k,v in pairs(self.tabObjects) do
		if(v) then 
			v:clean()
		end
	end

	self.tabObjects = nil
end

function copyTable(t)

	local t2 = {}
	for k,v in pairs(t) do
		
		if(type(v) == "table") then 
			t2[k] = copyTable(v)
			
		elseif(type(v) ~= "userdata")then
			t2[k] = v
		end

	end
	
	return t2
  
end

function printTable(t)

	for k,v in pairs(t) do
		print(k,v)
	end

	for i = 1,#t do
		print(t[i])
	end
  
end