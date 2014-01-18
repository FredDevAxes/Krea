--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description GraphicsCreator is a module loading all the image sheets and their config of your app 
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("graphicscreator", package.seeall)

--- 
-- Members of the GraphicsCreator Instance
-- @class table



GraphicsCreator = {}
GraphicsCreator.__index = GraphicsCreator

--- 
-- @description Create a new GraphicsCreator Instance
-- @return Return a new GraphicsCreator Instance
-- @param params Lua Table containing the params
-- @usage Usage:
-- <ul>
-- <li> local graphicsCreatorInstance = require("graphicscreator").GraphicsCreator.create(configName)</code></li>
-- </ul>

function GraphicsCreator.create(configName)
	
	--Init attributes of instance
	local graphicsCreator = {}           
	setmetatable(graphicsCreator,GraphicsCreator)
	
	
	graphicsCreator.configName = params.configName
	
	graphicsCreator.sheetLibrary = nil

	graphicsCreator:createGraphics()
	
	return graphicsCreator
	
end

function GraphicsCreator:createGraphics()
	if(self.configName) then 
		
		self.config = require(configName)
		
		self.sheetLibrary = {}
		
		local imageSheets = self.config.imageSheets
		for k,v in pairs(imageSheets) do 
			
			self.sheetLibrary[k] = {}
			self.sheetLibrary[k].sheet = graphics.newImageSheet( v.fileName, v.options )
			
			if(v.sequenceData) then 
				self.sheetLibrary[k].sequenceData = v.sequenceData
			end
		end
	
		self.objectConfigs = self.config.displayObjects
		
		self.config = nil
		
	end
	
end

