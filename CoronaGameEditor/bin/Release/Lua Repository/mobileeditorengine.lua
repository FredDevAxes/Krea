--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description MobileEditorEngine is an engine allowing the user to edit during execution the content of a TilesMap.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("mobileeditorengine", package.seeall)

MobileEditorEngine = {}
MobileEditorEngine.__index = MobileEditorEngine

local tilesMap = require("tilesmap").TilesMap
--ex params MobileEditorEngine
-- params = {
--tilesMapName = "map"
	-- tilesMapXPos = 5
	-- tilesMapYPos = 5
	 -- displayGroup = displayGroupLayer1
	-- }

--- 
-- @description Create a new TilesMap Mobile Editor Engine Instance
-- @return Return a new TilesMap Mobile Editor Engine Instance
-- @param params Lua Table containing the params
-- @see object
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.tilesMapName = "map0"
-- <br>params.tilesMapXPos = 10
-- <br>params.tilesMapYPos = 10 
-- <br>params.displayGroup = displayGroupLayer1</li>
-- <li> local mobileEditorEngine = require("mobileeditorengine").MobileEditorEngine.create(params)</code></li>
-- </ul>
function MobileEditorEngine.create(params)
	local editor = {}             -- our new object
	setmetatable(editor,MobileEditorEngine)  -- make Account handle lookup

	editor.tilesMapXPos = params.tilesMapXPos
	editor.tilesMapYPos = params.tilesMapYPos
	editor.displayGroup = params.displayGroup
	editor.mode = nil -- "EDITION" or "SCROLL" 
	editor.targetMode = nil -- "TEXTURES" or "OBJECTS" or "COLLISION"
	editor.editionMode = nil -- "REMOVE" or "APLLY"
	editor.selectionMode = nil -- "ONEBYONE" or "RECTANGLE" or "ALL"
	
	editor.currentTilesMap = nil
	
	editor.panelTilesModel = nil
	
	if(params.tilesMapName) then 
		editor:loadTilesMap(params.tilesMapName)
	end
	
	return editor
end

--- 
-- @description Start the editor engine
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:start()</code></li>
-- </ul>
function MobileEditorEngine:start()
	if(self.currentTilesMap) then 
		self.currentTilesMap:start()
		self.currentTilesMap.displayGroup:addEventListener("touch",self.currentTilesMap.onTouchTilesMap)
	end
	
	if(self.panelTilesModel) then 
		self.panelTilesModel:start()

	end
	
	if(self.targetMode == "COLLISIONS") then 
		self:showCollisions()
	end
end

--- 
-- @description Pause the editor engine
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:pause()</code></li>
-- </ul>
function MobileEditorEngine:pause()
	if(self.currentTilesMap) then 
		self.currentTilesMap:pause()
		self.currentTilesMap.displayGroup:removeEventListener("touch",self.currentTilesMap.onTouchTilesMap)
	end
	
	if(self.panelTilesModel) then 
		self.panelTilesModel:pause()

	end
	
	if(self.targetMode == "COLLISIONS") then 
		self:hideCollisions()
	end
	
end

--- 
-- @description Clean the Mobile Editor Engine Instance
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:clean()</li>
-- <li>mobileEditorEngine = nil</code></li>
-- </ul>
function MobileEditorEngine:clean()
	
	if(self.currentTilesMap) then 
		self.currentTilesMap:clean()
		self.currentTilesMap = nil
	end
	
	self.mode = nil -- "EDITION" or "SCROLL" 
	self.targetMode = nil -- "TEXTURES" or "OBJECTS" or "COLLISION"
	self.editionMode = nil -- "REMOVE" or "APLLY"
	self.selectionMode = nil -- "ONEBYONE" or "RECTANGLE" or "ALL"
	
	if(self.panelTilesModel) then 
		self.panelTilesModel:clean()
		self.panelTilesModel = nil
	end
end

--- 
-- @description Load a tilesmap to edit
-- @param tilesMapName The name of the tilesmap to edit
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:loadTilesMap(tilesMapName)</code></li>
-- </ul>
function MobileEditorEngine:loadTilesMap(tilesMapName)

	if(self.currentTilesMap) then 
	
		self.currentTilesMap:clean()
		self.currentTilesMap = nil
	end
	
	local params = {}
	params.name = tilesMapName
	params.xPos = self.tilesMapXPos
	params.yPos = self.tilesMapYPos
	params.displayGroup = self.displayGroup
	
	self.currentTilesMap = tilesMap.create(params)
	
	if(self.currentTilesMap) then 
		
		--Set the correct modes
		self:setModeEdition()
		
		--Create a panel containing all the textures and objects
		self.panelTilesModel = PanelTileModels.create(self)
		
		self:setModeTexturesTarget()
		
		--Attach a tuch listener to the tilesmap group
		local absol = math.abs
		
		self.currentTilesMap.onTouchTilesMap = function(event)
			local phase = event.phase

			if(self.mode == "EDITION") then 
				if(self.targetMode == "COLLISIONS") then 
					if(phase == "began") then 
					local tile = self.currentTilesMap:getTile(event.x,event.y)
						if(tile) then 
							if(tile.isCrossable == false) then 
								self.currentTilesMap.tabTilesCollisionProp[tile.currentIndexInTabs] = 0
								tile.isCrossable = true
							else
								self.currentTilesMap.tabTilesCollisionProp[tile.currentIndexInTabs] = 1
								tile.isCrossable = false
							end
							self:showCollisions()
						end
					end
				else
					local modelSelected = self.panelTilesModel.tileModelSelected
					if(self.editionMode == "APPLY") then
						if(modelSelected) then 
							local tile = self.currentTilesMap:getTile(event.x,event.y) 
							self:applyModelToTile(tile)
						end
					elseif(self.editionMode == "REMOVE") then
						self.panelTilesModel.tileModelSelected = nil
						local tile = self.currentTilesMap:getTile(event.x,event.y) 
						self:applyModelToTile(tile)
					end
				end
				
			elseif(self.mode == "SCROLL") then 
				local map = self.currentTilesMap
				if phase == "began" then
					map.prevX = event.x
					map.prevY = event.y
				elseif phase == "moved" then
					if(event.x ~= map.prevX or event.y ~= map.prevY) then 
						local xOffSet = map.prevX - event.x
						local yOffSet = map.prevY - event.y
						
						if(absol(xOffSet) >=10 or absol(yOffSet) >=10) then 
							map.xScroll = map.xScroll + xOffSet
							map.yScroll = map.yScroll + yOffSet
							map:updateDisplay(map.prevX - event.x + map.xOrigin,map.prevY - event.y+ map.yOrigin)
							map.prevX = event.x
							map.prevY = event.y
							
							if(self.targetMode == "COLLISIONS") then 
								self:showCollisions()
							end
						end
					end
				end
			end
		end
		
		
		
	end
	
	
	
end

--- 
-- @description Save the tilesmap which has been edited
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:saveTilesMap()</code></li>
-- </ul>
function MobileEditorEngine:saveTilesMap()
	if(self.currentTilesMap) then 
		
		local result = self.currentTilesMap:saveFramesIndexTextures()
		print(result)
		
		result = self.currentTilesMap:saveFramesObjectsTextures()
		print(result)
		
		result = self.currentTilesMap:saveCollisionContentFile()
		print(result)
		
		--Update timeFile
		local date = os.date( "*t" ) 
		local seconds = getSecondsFromDateSince2000(date.year,date.month,date.day,date.hour,date.min,date.sec)
		print(seconds)
		
		local finalPath = system.pathForFile( self.currentTilesMap.name.."lastmodifiedtime.txt", system.DocumentsDirectory)
		local file = io.open( finalPath, "w+" )
		local result2 = file:write(seconds)
		file:close()
		if(not result2) then 
			print("TimeFile update error !")
		else
			print("TimeFile updated !")
		end 
	end
end

--- 
-- @description Apply the current tile model selected to a tile
-- @param tile The tile to affect the model
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:applyModelToTile(tile)</code></li>
-- </ul>
function MobileEditorEngine:applyModelToTile(tile)

	local currentTilesMap = self.currentTilesMap
	if(currentTilesMap) then 
		if(tile) then 
			if(self.panelTilesModel.tileModelSelected) then 
				local frame = self.panelTilesModel.tileModelSelected.currentFrame
				local targetMode = self.targetMode
				
				if(targetMode == "TEXTURES") then 
					currentTilesMap.tabFramesTextures[tile.currentIndexInTabs] = frame
					tile.texture.currentFrame = frame
					tile.texture.isVisible = true
				elseif(targetMode == "OBJECTS") then 
					currentTilesMap.tabFramesObjects[tile.currentIndexInTabs] = frame
					tile.imageObject.currentFrame = frame
					tile.imageObject.isVisible = true
				end
			else
				if(targetMode == "TEXTURES") then 
					currentTilesMap.tabFramesTextures[tile.currentIndexInTabs] = "N"
					tile.texture.isVisible = false
				elseif(targetMode == "OBJECTS") then 
					currentTilesMap.tabFramesObjects[tile.currentIndexInTabs] = "N"
					tile.imageObject.isVisible = false
				end
			end
		end
	end
end

--- 
-- @description Set the current editor engine mode to "SCROLL"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeScrolling()</code></li>
-- </ul>
function MobileEditorEngine:setModeScrolling()
	self.mode = "SCROLL"
end

--- 
-- @description Set the current editor engine mode to "EDITION"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeEdition()</code></li>
-- </ul>
function MobileEditorEngine:setModeEdition()
	self.mode = "EDITION"
end

--- 
-- @description Set the tile layer target to "TEXTURES"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeTexturesTarget()</code></li>
-- </ul>
function MobileEditorEngine:setModeTexturesTarget()
	self.targetMode = "TEXTURES"
	self.panelTilesModel:showModels("TEXTURES")
end

--- 
-- @description Set the tile layer target to "OBJECTS"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeObjectsTarget()</code></li>
-- </ul>
function MobileEditorEngine:setModeObjectsTarget()
	self.targetMode = "OBJECTS"
	self.panelTilesModel:showModels("OBJECTS")
end

--- 
-- @description Set the tile layer target to "COLLISIONS"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeCollisionTarget()</code></li>
-- </ul>
function MobileEditorEngine:setModeCollisionTarget()
	self.targetMode = "COLLISIONS"
	self:showCollisions()
end

--- 
-- @description Set Editor engine edition mode to "APPLY"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeApply()</code></li>
-- </ul>
function MobileEditorEngine:setModeApply()
	self.editionMode = "APPLY"
end

--- 
-- @description Set Editor engine edition mode to "REMOVE"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setModeRemove()</code></li>
-- </ul>
function MobileEditorEngine:setModeRemove()
	self.editionMode = "REMOVE"
end

--- 
-- @description Set the selection mode to "ONE"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setOneSelectionMode()</code></li>
-- </ul>
function MobileEditorEngine:setOneSelectionMode()
	self.selectionMode = "ONE"
end

--- 
-- @description Set the selection mode to "ALL"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setAllSelectionMode()</code></li>
-- </ul>
function MobileEditorEngine:setAllSelectionMode()
	self.selectionMode = "ALL"
end

--- 
-- @description Set the selection mode to "RECTANGLE"
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:setRectangleSelectionMode()</code></li>
-- </ul>
function MobileEditorEngine:setRectangleSelectionMode()
	self.selectionMode = "RECTANGLE"
end

--- 
-- @description Display the tiles that are not crossable
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:showCollisions()</code></li>
-- </ul>
function MobileEditorEngine:showCollisions()
	self:hideCollisions()
	
	local currentTilesMap = self.currentTilesMap
	if(currentTilesMap) then
		self.displayGroupCollisions = display.newGroup()
		self.displayGroup:insert(self.displayGroupCollisions)
		
		local tabTiles = currentTilesMap.tabTiles
		for i = 1,#tabTiles do 
			local tile = tabTiles[i]
			if(tile) then 
				if(tile.isCrossable == false) then 
					local marquer = display.newRect(tile.texture.x,tile.texture.y,currentTilesMap.tileWidth,
									currentTilesMap.tileHeight)
					self.displayGroupCollisions:insert(marquer)
					marquer.x = tile.texture.x
					marquer.y = tile.texture.y
					marquer:setFillColor(255,0,0)
					marquer.alpha = 0.3
					
				end
			end
		end
	end
end

--- 
-- @description Hide the tiles that are not crossable
-- @usage Usage:
-- <ul>
-- <li><code>mobileEditorEngine:hideCollisions()</code></li>
-- </ul>
function MobileEditorEngine:hideCollisions()
	if(self.currentTilesMap) then 
		
		if(self.displayGroupCollisions) then 
			self.displayGroupCollisions:removeSelf()
		end
		
		self.displayGroupCollisions = nil
	end
end


----- Class PanelTileModels ----------
PanelTileModels = {}
PanelTileModels.__index = PanelTileModels


function PanelTileModels.create(editorEngine)
	local panel = {}             -- our new object
	setmetatable(panel,PanelTileModels)  -- make Account handle lookup
	
	panel.editorEngine = editorEngine
	
	--Init attributes
	panel.tileModelSelected = nil
	
	panel.tabTileModels = nil
	panel.currentTileModelsMode = nil -- "TEXTURES" or "OBJECTS"
	panel.lastPos = nil
	
	--Init display
	panel.surface = display.newRect(display.contentWidth - 100,0,
							100,display.contentHeight)
	panel.surface:setFillColor(0,0,0)
	panel.surface.alpha = 0.8
	panel.surface:setReferencePoint(display.TopLeftReferencePoint)
	panel.editorEngine.displayGroup:insert(panel.surface)
	
	panel.displayGroupModels = display.newGroup()
	panel.editorEngine.displayGroup:insert(panel.displayGroupModels)
	
	panel.onTouchSurface = function(event)
		local phase = event.phase

		if(phase == "began") then 
			panel.lastPos = event.y
			
		elseif(phase=="moved" or phase=="ended" ) then 
			if(panel.lastPos ) then 
				local offSet = event.y - panel.lastPos 

				panel.displayGroupModels.y = panel.displayGroupModels.y + offSet
			end
			panel.lastPos = event.y
			
		end
		
		return true
	end
	
	
	return panel
	
	
end

function PanelTileModels:start()
	self.surface:addEventListener("touch",self.onTouchSurface)
end

function PanelTileModels:pause()
	self.surface:removeEventListener("touch",self.onTouchSurface)
end

function PanelTileModels:clean()
	self:cleanTilesModel()
	
	if(self.surface.removeSelf) then 
		self.surface:removeSelf()
		self.surface= nil
	end
	
	self.displayGroupModels:removeSelf()
	self.tileModelSelected = nil
	self.tabTileModels = nil
	self.currentTileModelsMode = nil
end

function PanelTileModels:cleanTilesModel()
	local tabTileModels = self.tabTileModels
	if(tabTileModels) then 
		for i = 1,#tabTileModels do 
			local tileModel = tabTileModels[i]
			tileModel:removeSelf()
			
			tileModel = nil
		end
		
		tabTileModels = nil
		self.tabTileModels = nil
		self.currentTileModelsMode = nil
		
		if(self.selectionRect.removeSelf) then 
			self.selectionRect:removeSelf()
			self.selectionRect = nil
		end
	end
end

function PanelTileModels:setSelectedTileModel(tileModel)
	self.tileModelSelected = tileModel
	
	if(tileModel) then 
		if(tileModel.removeSelf) then 
			self.selectionRect.x = tileModel.x
			self.selectionRect.y = tileModel.y
		end
	end
	
end

function PanelTileModels:scrollTileModel(offset)
	
	local tabTileModels = self.tabTileModels
	if(tabTileModels) then 
		for i = 1,#tabTileModels do 
			local model = tabTileModels[i]
			model.y = model.y + offset
		end
	end
end

--"TEXTURES" ou "OBJECTS"
function PanelTileModels:showModels(target)
	local currentTilesMap = self.editorEngine.currentTilesMap
	if(currentTilesMap) then 
	
		if(self.currentTileModelsMode) then 
			if(self.currentTileModelsMode == target) then return end
		end
	
		if(self.tabTileModels) then 
			self:cleanTilesModel()
		end
		
		local count = nil
		local textureSet = nil
		if(target == "TEXTURES") then 
			count = currentTilesMap.infos.texturesCount
			textureSet = currentTilesMap.textureSet
		elseif(target == "OBJECTS") then 
			count = currentTilesMap.infos.objectsCount
			textureSet = currentTilesMap.imageObjectSet
		end
		
		local onTouchTileModel = function(event)
			local phase = event.phase
			local target = event.target
			
			if(phase == "began") then 
				self:setSelectedTileModel(target)
			end
			
		end
		
		local tileWidth = currentTilesMap.tileWidth
		local tileHeight = currentTilesMap.tileHeight
		local xTileModelsInView = math.floor(self.surface.contentWidth / tileWidth)
		local columnIndex = -1
		local lineIndex = 0
		
		self.tabTileModels = {}
		--Create sprites
		for i = 1, count do 
			local model = sprite.newSprite(textureSet)
			model:setReferencePoint(display.TopLeftReferencePoint)
			table.insert(self.tabTileModels,model)
			
			self.displayGroupModels:insert(model)
			
			
			columnIndex = columnIndex +1
			if(columnIndex > xTileModelsInView-1) then 
				columnIndex = 0
				lineIndex =lineIndex+1
			end
			
			model.x = self.surface.x + columnIndex * tileWidth
			model.y = self.surface.y + lineIndex * tileHeight
			
			model.currentFrame = i
			model:addEventListener("touch",onTouchTileModel)
		end

		--Create the rect of selection
		if(#self.tabTileModels >=1) then
			self.selectionRect = display.newRect(self.tabTileModels[1].x,self.tabTileModels[1].y,tileWidth,tileHeight)
			self.selectionRect:setReferencePoint(display.TopLeftReferencePoint)
			self.selectionRect:setFillColor(255,255,128)
			self.selectionRect.alpha = 0.3
			self.displayGroupModels:insert(self.selectionRect)
		end	
		
		self.currentTileModelsMode = target
	end
end

----------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------
--------------- DATETIME UTILITY FUNCTIONS ----------------------------------
-----------------------------------------------------------------------------

function getSecondsFromDateSince2000(year,month,day,hour,minute,seconds)

	--Get Seconds of 1/01/2000
	local startSeconds = getSecondsFromDate(2000,1,1,0,0,0)
	local endSeconds = getSecondsFromDate(year,month,day,hour,minute,seconds)
	local result = endSeconds - startSeconds
	return result
end

function getSecondsFromDate(year,month,day,hour,minute,second)
	local YEAR = 31556926
	local MONTH = 2629743
	local DAY = 86400
	local HOUR = 3600
	local MIN = 60
	
	local result = year * YEAR + month * MONTH + day * DAY + hour * HOUR + minute * MIN + second
	return result
end
