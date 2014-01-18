--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description Tilesmap is a module providing the possibility to add into a project the concept of TilesMap.
-- <br>This one has a real interest if you want a big world in your project without using too much the memory.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("tilesmap", package.seeall)

local sprite = require("sprite")

--- 
-- Members of the Tiles Map Instance
-- @class table
-- @name Fields
-- @field displayGroup The Corona display group where each tile is stored
-- @field name The tiles map name
-- @field isInfinite Boolean which indicates whether the tiles map is infinite or not
-- @field infos A table containing informations about the tilles map
-- @field isPhysicsEnabled Boolean which indicates whether the tiles map interacts with physics
-- @field nbTilesX The tiles count on X axis
-- @field nbTilesY The tiles count on Y axis
-- @field tileWidth The width of each tile
-- @field tileHeight The height of each tile
-- @field textureSet The sprite set containing all textures (First tile layer)
-- @field imageObjectSet The sprite set containing all objects textures (Second tile layer)
-- @field texturesInitFilePath The path of the textures initialisation file
-- @field objectsInitFilePath The path of the objects initialisation file
-- @field tilesCollisionInitFilePath The path of the collisions initialisation file
-- @field xScroll The current value of the tiles map scroll on the X axis
-- @field yScroll The current value of the tiles map scroll on the Y axis
-- @field isPaused Indicates if the tiles map is in a paused state
-- @field tabTiles The table containing each tile (Corona sprite object)
-- @field tabObjectsInInteraction The table containing all external display objects handled by the tiles map
-- @field xOrigin The X origin of the tiles map
-- @field yOrigin The Y origin of the tiles map
-- @field xTilesInView The number of tile that can be viewed from the screen on X axis
-- @field yTilesInView The number of tile that can be viewed from the screen on Y axis


----- Class TilesMap ----------
TilesMap = {}
TilesMap.__index = TilesMap

local floor = math.floor
local abs = math.abs
local ceil = math.ceil

local screenOriginX = display.screenOriginX
local screenOriginY = display.screenOriginY
local contentWidth = display.contentWidth
local contentHeight = display.contentHeight

local halfContentWidth = contentWidth * 0.5
local halfContentHeight = contentHeight * 0.5

--- 
-- @description Create a new TilesMap Engine Instance
-- @return Return a TilesMap Engine Instance
-- @param params Lua Table containing the params
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.displayGroupParent = layer1
-- <br>params.isInfinite = true
-- <br>params.xPos = 0
-- <br>params.yPos = 0</li>
-- <li> local tilesMapEngine = require("tilesmap").TilesMap.create(params)</code></li>
-- </ul>
function TilesMap.create(params)
	local tilesMap = {}             -- our new object
	setmetatable(tilesMap,TilesMap)  -- make Account handle lookup

	tilesMap.displayGroup = params.displayGroup
	tilesMap.name = params.name
	tilesMap.isInfinite = params.isInfinite
	
	
	--Init tilesmap ressource file 
	tilesMap.infos = require(tilesMap.name.."infos").getInfos()
	tilesMap.isPhysicsEnabled = tilesMap.infos.isPhysicsEnabled
	tilesMap.nbTilesX = tilesMap.infos.nbWidth
	tilesMap.nbTilesY = tilesMap.infos.nbHeight
	tilesMap.tileWidth = tilesMap.infos.tileWidth
	tilesMap.tileHeight = tilesMap.infos.tileHeight
	if(tilesMap.infos.texturesCount>0) then 
		local tabSpriteSet = {}
		for i=0,tilesMap.infos.textureSheetCount -1 do
			tabSpriteSet[i+1] = {}
			local texturesData = require(tilesMap.name.."textsheet"..i.."data").getSpriteSheetData()
			local pngName = tilesMap.name.."textsheet"..i..".png"
			local texturesSheet = sprite.newSpriteSheetFromData( pngName, texturesData )
			tabSpriteSet[i+1].sheet = texturesSheet
			
			local frames = {}
			for j =1,tilesMap.infos.textureCountBySheet[i+1] do 
				table.insert(frames,j)
			end
			tabSpriteSet[i+1].frames = frames
		end
		
		tilesMap.textureSet = sprite.newSpriteMultiSet(tabSpriteSet)
		
	end

	if(tilesMap.infos.objectsCount>0) then 
		local tabSpriteSet = {}
		for i=0,tilesMap.infos.objectSheetCount -1 do
			tabSpriteSet[i+1] = {}
			local objectsData = require(tilesMap.name.."objsheet"..i.."data").getSpriteSheetData()
			local pngName = tilesMap.name.."objsheet"..i..".png"
			local objectSheet = sprite.newSpriteSheetFromData( pngName, objectsData )
			tabSpriteSet[i+1].sheet = objectSheet

			
			local frames = {}
			for j =1,tilesMap.infos.objectCountBySheet[i+1] do 
				table.insert(frames,j)
			end
			tabSpriteSet[i+1].frames = frames
		end
		
		tilesMap.imageObjectSet = sprite.newSpriteMultiSet(tabSpriteSet)
		
	end
	
	tilesMap.texturesInitFilePath = tilesMap.infos.texturesInitFilePath
	tilesMap.objectsInitFilePath = tilesMap.infos.objectsInitFilePath
	tilesMap.tilesCollisionInitFilePath = tilesMap.infos.collisionInitFilePath
	
	
	tilesMap.xScroll = params.xPos
	tilesMap.yScroll = params.yPos
	
	
	
	tilesMap.isPaused = true
	tilesMap.tabTiles = {}
	
	tilesMap.tabObjectsInInteraction = {}
	
	tilesMap.xOrigin = params.xPos 
	tilesMap.yOrigin = params.yPos 
	
	
	tilesMap:loadContentFiles()
	tilesMap.xTilesInView = floor((contentWidth -screenOriginX) / tilesMap.tileWidth) +5
	tilesMap.yTilesInView = floor((contentHeight  -screenOriginY) / tilesMap.tileHeight)+5

	tilesMap.prevX = 0
	tilesMap.prevY = 0
	tilesMap.currentX = 0
	tilesMap.currentY = 0
	
	
	tilesMap:populate()
	tilesMap:updateDisplay(tilesMap.xOrigin,tilesMap.yOrigin)

	if(tilesMap.infos.isPathFindingEnabled == true) then 
		local paramsFStar = {}
		paramsFStar.tilesMapParent = tilesMap
		
		tilesMap.fStar = require("fstar").FStar.create(paramsFStar)
		
	end
	
	tilesMap.drag = function(event)
        if event.phase == "began" then
                tilesMap.prevX = event.x
                tilesMap.prevY = event.y
        elseif event.phase == "moved" then
				local diffX = math.floor(tilesMap.prevX - event.x)
				local diffY = math.floor(tilesMap.prevY - event.y)
				
				tilesMap.xScroll = tilesMap.xScroll + diffX
				tilesMap.yScroll = tilesMap.yScroll + diffY
				tilesMap:updateDisplay(diffX + tilesMap.xOrigin,diffY+ tilesMap.yOrigin)
				tilesMap.prevX = event.x
				tilesMap.prevY = event.y	
				
        end
	end
	
	
	return tilesMap
end

--- 
-- @description Start the TilesMap refresh by loading its content from the contentTextFile needed
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:start()</code></li>
-- </ul>
function TilesMap:start()
	self.isPaused = false
	self:loadContentFiles()
	self:updateDisplay(0,0)

	if(self.objectToFolow) then 
		if(self.Scroll) then 
			Runtime:addEventListener("enterFrame",self.Scroll)
		end
		
	end
end

--- 
-- @description Pause the TilesMap refresh
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:pause()</code></li>
-- </ul>
function TilesMap:pause()
	self.isPaused = true
	self:setAutoScrollInactive()
end

--- 
-- @description Load the content of the tilesmap by loading data from contentFiles
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadContentFiles()</code></li>
-- </ul>
function TilesMap:loadContentFiles()
	--Init frames index
	if(self.textureSet) then 
		self:loadFramesIndexTextures()
	end
	
	if(self.imageObjectSet) then 
		self:loadFramesIndexImageObjects()
	end
	
	--Init collisions
	if(self.isPhysicsEnabled == true) then 
		self:loadTilesCollisionFile()
	end
end

--- 
-- @description Populate the grid of tiles by rectangle sprites
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:populate()</code></li>
-- </ul>
function TilesMap:populate()
	--populate the group with just enough objects to cover the entire screen
	local friction = self.infos.TileFriction
	local density = self.infos.TileDensity
	local bounce = self.infos.TileBounce
	local radius = self.infos.TileRadius
	local collisionCatBits = self.infos.CollisionCategoryBits
	local collisionMaskBits = self.infos.CollisionMaskBits
	local halfTileWidth = self.tileWidth*0.5
	local halfTileHeight = self.tileHeight*0.5
	for y = 0, self.yTilesInView -1 do
        for x = 0, self.xTilesInView -1 do
			
			--Set tile location
			
			local xDest = (x * self.tileWidth) +screenOriginX+halfTileWidth
			local yDest = (y * self.tileHeight)  +screenOriginY+halfTileHeight
			local tile = Tile.create(self,xDest,yDest,density,bounce,friction,radius,collisionCatBits,collisionMaskBits)
			tile.yIndex = y 
			tile.xIndex = x 
			
			self.tabTiles[#self.tabTiles +1] =tile
        end
	end
end

--- 
-- @description Add an object instance (see object) to the list of object the tilesmap should interacts with. This make an object handled by the tilesmap.
-- @param objectInstance An object previoulsy created via require("object").Object.create(params)
-- @see object
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:addExternalObjectToInteractWith(objectInstance)</code></li>
-- </ul>
function TilesMap:addExternalObjectToInteractWith(objectInstance)
	
	if(self.tabObjectsInInteraction) then 
		self.tabObjectsInInteraction[#self.tabObjectsInInteraction+1] = objectInstance
	end
end

--- 
-- @description Align and update the display object grid based on the current scroll position
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:updateDisplay()</code></li>
-- </ul>
-- 
function TilesMap:updateDisplay()
  local currentScrollX = self.xScroll-self.tileWidth*0.5
  local currentScrollY = self.yScroll -self.tileHeight*0.5
  local diffX =  - (currentScrollX % self.tileWidth)
  local diffY =  - (currentScrollY % self.tileHeight)

  -- update the tile contents
  local scrollX = floor((currentScrollX ) / self.tileWidth) 
  local scrollY = floor((currentScrollY ) / self.tileHeight)

  local xMove = currentScrollX+diffX
  local yMove = currentScrollY+diffY
  
  local tabTiles = self.tabTiles
  local xTilesInView = self.xTilesInView
  local yTilesInView = self.yTilesInView
  local nbTilesX = self.nbTilesX
  local nbTilesY = self.nbTilesY

  for y = 0, yTilesInView -1 do
			for x = 0, xTilesInView -1 do

				local tx = scrollX + x;
				local ty = scrollY + y;
				
				local tile = tabTiles[xTilesInView * y + x + 1]
				tile:move(xMove,yMove)
				
				if(self.isInfinite == true) then 
					--Rendre la map infinie
					if(tx >= nbTilesX ) then 
						local loopX = floor(tx / nbTilesX) 
						local nbTilesXLoops = nbTilesX *  loopX
						tx = tx - nbTilesXLoops 
					elseif(tx < 0) then 
						local loopX = floor(tx / nbTilesX) 
						local nbTilesXLoops = nbTilesX *  loopX
						tx = math.abs(nbTilesXLoops - tx)

					end
					
					if(ty >= nbTilesY) then 
						local loopY = floor(ty / nbTilesY)
						local nbTilesYLoops = nbTilesY *  loopY
	 
						ty = ty - nbTilesYLoops
					elseif(ty <0) then 
						local loopY = floor(ty / nbTilesY)
						local nbTilesYLoops = nbTilesY *  loopY
	 
						ty = abs(nbTilesYLoops - ty)
					end
				end
				
				
				--Move the tile
				self:tileVisibleTest(tile, tx, ty)
				
			end
	end
	
	
	
	self:objectsVisibleTest(diffX,diffY)
end

--- 
-- @description Move the tiles by the x and y offset
-- @param xOffset The offset on the x-axis
-- @param yOffset The offset on the y-axis
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:updateDisplay()</code></li>
-- </ul>

function TilesMap:moveTiles(xOffset,yOffset)

	local tabTiles = self.tabTiles
	if(tabTiles) then 
		for i = 1,#tabTiles do 
			local tile = tabTiles[i]
			if(tile) then 
				tile:move(xOffset,yOffset)
				
			end
			
		end
	end
end

function TilesMap:objectsVisibleTest(diffX,diffY)
	local tabObjectsInInteraction = self.tabObjectsInInteraction
	if(tabObjectsInInteraction) then 
		local tabTiles = self.tabTiles
		local halfTilesWidth = self.tileWidth * 0.5
		local halfTilesHeight = self.tileHeight * 0.5
		local xTopLeft = tabTiles[1].texture.x - halfTilesWidth - diffX
		local yTopLeft = tabTiles[1].texture.y -halfTilesHeight-diffY
		local xTopRight = tabTiles[self.xTilesInView].texture.x -self.tileWidth -diffX
		
		local yBottomRight = tabTiles[self.yTilesInView * self.xTilesInView ].texture.y-self.tileHeight-diffY
		

		
		for i = 1,#tabObjectsInInteraction do 
			
			local objectInstance = tabObjectsInInteraction[i]
			
			if(objectInstance.object.removeSelf) then 
				-- Si l'object est dans la tilesmap
				
				if(objectInstance.object.x  +objectInstance.object.contentWidth >= xTopLeft
				and objectInstance.object.x < xTopRight
				and objectInstance.object.y +objectInstance.object.contentHeight>= yTopLeft
				and objectInstance.object.y < yBottomRight) then 
					
					objectInstance:startInteractions()
				else

					objectInstance:pauseInteractions()
				end
			end
		end
		


	end
end

--- 
-- @description Get a tile instance at loaction X and Y
-- @param locationX The location on the x-axis
-- @param locationY The location on the y-axis
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:getTile(locationX,locationY) </code></li>
-- </ul>
function TilesMap:getTile(locationX,locationY) 
	local tabTiles = self.tabTiles 
	local halfTilesWidth = self.tileWidth * 0.5
	local halfTilesHeight = self.tileHeight * 0.5
	for i = 1, #tabTiles do 
		local tile = tabTiles[i]
		if(locationX >= tile.texture.x - halfTilesWidth and locationX <= tile.texture.x + halfTilesWidth
			and locationY >= tile.texture.y - halfTilesHeight and locationY <= tile.texture.y + halfTilesHeight) then 
				return tile
		end
	end
	
	return nil
end

function TilesMap:tileVisibleTest(t,x,y)
	
	if(x >= 0 and x < self.nbTilesX and y >= 0 and y < self.nbTilesY) then
		--Show texture if exists
		
		local indexTileInTabs = y * self.nbTilesX + x +1
		t.currentIndexInTabs = indexTileInTabs
		
		if(self.textureSet) then 
			local indexFrameTexture = self.tabFramesTextures[indexTileInTabs]
			if(indexFrameTexture ~= "N") then 
				t.texture.isVisible = true
				t.texture.currentFrame = tonumber(indexFrameTexture)
				
			else
				t.texture.isVisible = false
			end
		end
		
		--Show object if exists
		if(self.imageObjectSet) then 
			local indexFrameObject = self.tabFramesObjects[indexTileInTabs]
			if(indexFrameObject ~= "N") then 
				t.imageObject.isVisible = true
				t.imageObject.currentFrame = tonumber(indexFrameObject)
			else
				t.imageObject.isVisible = false
			end
		end
		
		
		-- Get the collisionning for this tile
		if(self.isPhysicsEnabled == true) then 
			if(self.isPaused == false) then 
				local isCrossable = tonumber(self.tabTilesCollisionProp[indexTileInTabs])
				if(isCrossable) then 
					if(isCrossable ==1) then 
						t.isCrossable = false
						t.texture.isBodyActive = true
					else
						t.isCrossable = true
						t.texture.isBodyActive = false

					end
				end
			else
				t.isCrossable = true
				t.texture.isBodyActive = false
			end
		end
		
		
	else
			-- tile is off the edge of the self
			if(self.isPhysicsEnabled == true) then 
				t.texture.isBodyActive = false
			end
			t.texture.isVisible = false
			
			if(t.imageObject) then 
				t.imageObject.isVisible = false
			end
	end
end

--- 
-- @description Set the auto scroll active by following an object passed in parameter
-- @param objectToFolow An object to follow previoulsy created via require("object").Object.create(params)
-- @see object
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:setAutoScrollActive(objectInstance)</code></li>
-- </ul>
function TilesMap:setAutoScrollActive(objectToFolow)
	self:setDragInactive()
	if(objectToFolow) then 
		if(objectToFolow.object.removeSelf) then 
			
			self.objectToFolow = objectToFolow
			
			local parent = objectToFolow.object.parent
			self.Scroll = function (event)
				self.currentX = parent.x
				self.currentY = parent.y
				
				if(self.prevX ~= self.currentX or self.prevY ~= self.currentY) then 
				
					local xOffSet = self.prevX - self.currentX
					local yOffSet = self.prevY - self.currentY
					self.xScroll = self.xScroll + xOffSet
					self.yScroll = self.yScroll + yOffSet
							
					self:updateDisplay(self.xScroll,self.yScroll)
				end
			
				self.prevX = self.currentX 
				self.prevY = self.currentY 
				
				
			end
			
			
			Runtime:addEventListener("enterFrame",self.Scroll)
		end
		
	end
end

--- 
-- @description Set the auto scroll inactive
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:setAutoScrollInactive()</code></li>
-- </ul>
function TilesMap:setAutoScrollInactive()
	if(self.objectToFolow) then 
		if(self.Scroll) then 
			Runtime:removeEventListener("enterFrame",self.Scroll)
		end
	end
end

--- 
-- @description Set the drag active
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:setDragActive()</code></li>
-- </ul>
function TilesMap:setDragActive()
	if(self.isDragActive == false or self.isDragActive == nil)  then 
		self.displayGroup:addEventListener("touch", self.drag)
		self.isDragActive = true
		
	end
	
end

--- 
-- @description Set the drag inactive
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:setDragInactive()</code></li>
-- </ul>
function TilesMap:setDragInactive()
	if(self.isDragActive == true) then 
		self.displayGroup:removeEventListener("touch", self.drag)
		self.isDragActive = false
	end
end

--- 
-- @description Save the current Texture tiles state
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:saveFramesIndexTextures()</code></li>
-- </ul>
function TilesMap:saveFramesIndexTextures()
	local finalPath = system.pathForFile( self.texturesInitFilePath, system.DocumentsDirectory )
	
	
	local texturesContent = ""
	
	local nbTilesX = self.nbTilesX 
	local nbTilesY = self.nbTilesY 
	local tabFramesTextures = self.tabFramesTextures
	
	for y = 0,nbTilesY -1 do 
		
		for x = 0,nbTilesX -1 do 
		
			if(x >= 0 and x < nbTilesX and y >= 0 and y < nbTilesY) then
				local indexTileInTabs = y * nbTilesX + x +1
				local indexFrameTexture = tabFramesTextures[indexTileInTabs]
				
				texturesContent = texturesContent.."i"..indexFrameTexture
			end
			
		end
		
		texturesContent = texturesContent.."i\n"
	end
	
	local file = io.open( finalPath, "w+" )
	local result = file:write(texturesContent)
	file:close()
	if(not result) then 
		return "Write error"
	else
		return "Write Success"
	end 
end

--- 
-- @description Load the Texture tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadFramesIndexTextures()</code></li>
-- </ul>
function TilesMap:loadFramesIndexTextures()
	
	self.tabFramesTextures = nil
	self.tabFramesTextures = {}
	
	local finalPath = system.pathForFile( self.texturesInitFilePath, system.DocumentsDirectory  )
	
	local lastIndexOf_ = 1
	local indexDep = nil
	local indexFin  = nil
	local indexFrame = nil
	local nbTilesX = self.nbTilesX 
	local nbTilesY = self.nbTilesY 
	
	local file = io.open( finalPath, "r" )
	if file then
		
		local lineCount = 0
		for line in file:lines() do
			
			if(lineCount == nbTilesY) then
				break
			end	
				
			lastIndexOf_ = 1
			
			for i = 1, nbTilesX do

				indexDep = line:find("i",lastIndexOf_)

				if(indexDep) then 
					indexFin = line:find("i",indexDep+1)

					if(indexFin) then 
						indexFrame = line:sub(indexDep+1,indexFin-1)
						lastIndexOf_ = indexFin
						
						-- Add index in tab
						self.tabFramesTextures[#self.tabFramesTextures+1] = indexFrame
					end
				end
			end
			
			lineCount = lineCount+1
		end	
		
		file:close()
	end
	
		
end

--- 
-- @description Load the object tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadFramesIndexImageObjects()</code></li>
-- </ul>
function TilesMap:loadFramesIndexImageObjects()

	self.tabFramesObjects = nil
	self.tabFramesObjects = {}
	
	local finalPath = system.pathForFile( self.objectsInitFilePath, system.DocumentsDirectory  )

	local lastIndexOf_ = 1
	local indexDep = nil
	local indexFin  = nil
	local indexFrame = nil
	
	local nbTilesX = self.nbTilesX 
	local nbTilesY = self.nbTilesY 
	
	local file = io.open( finalPath, "r" )
	if file then
		local lineCount = 0
		for line in file:lines() do
			
			if(lineCount == nbTilesY) then
				break
			end	
			lastIndexOf_ = 1
			
			for i = 1, nbTilesX do
			
				
				indexDep = line:find("i",lastIndexOf_)
				if(indexDep) then 
					indexFin = line:find("i",indexDep+1)
					if(indexDep) then 
						indexFrame = line:sub(indexDep+1,indexFin-1)
						lastIndexOf_ = indexFin
						
						-- Add index in tab
						self.tabFramesObjects[#self.tabFramesObjects+1] = indexFrame
					end
				end
			end
			
			lineCount = lineCount+1
		end	
		
		file:close()
	end
	
end	

--- 
-- @description Save the object tiles state
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:saveFramesObjectsTextures()</code></li>
-- </ul>
function TilesMap:saveFramesObjectsTextures()
	local finalPath = system.pathForFile( self.objectsInitFilePath, system.DocumentsDirectory)
	
	
	local objectsContent = ""
	local nbTilesX = self.nbTilesX 
	local nbTilesY = self.nbTilesY 
	
	for y = 0,nbTilesY -1 do 
		
		for x = 0,nbTilesX -1 do 
		
			if(x >= 0 and x < nbTilesX and y >= 0 and y < nbTilesY) then
				local indexTileInTabs = y * nbTilesX + x +1
				local indexFrameObject = self.tabFramesObjects[indexTileInTabs]
				
				objectsContent = objectsContent.."i"..indexFrameObject
			end
			
		end
		
		objectsContent = objectsContent.."i\n"
	end
	
	local file = io.open( finalPath, "w+" )
	local result = file:write(objectsContent)
	file:close()
	if(not result) then 
		return "Write error"
	else
		return "Write Success"
	end 
end

--- 
-- @description Load the collision tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadTilesCollisionFile()</code></li>
-- </ul>
function TilesMap:loadTilesCollisionFile()

	self.tabTilesCollisionProp = nil
	self.tabTilesCollisionProp = {}
	
	local finalPath = system.pathForFile( self.tilesCollisionInitFilePath, system.DocumentsDirectory  )

	local lastIndexOf_ = 1
	local indexDep = nil
	local indexFin  = nil
	local indexFrame = nil
	local nbTilesX = self.nbTilesX 
	local nbTilesY = self.nbTilesY 
	
	local file = io.open( finalPath, "r" )
	if file then
		local lineCount = 0

		for line in file:lines() do
			
			if(lineCount == nbTilesY) then
				break
			end	
			lastIndexOf_ = 1
			
			for i = 1, nbTilesX do
			
				
				indexDep = line:find("i",lastIndexOf_)
				if(indexDep) then 
					indexFin = line:find("i",indexDep+1)
					if(indexFin) then 
						indexFrame = line:sub(indexDep+1,indexFin-1)
						lastIndexOf_ = indexFin
						
						-- Add index in tab
						self.tabTilesCollisionProp[#self.tabTilesCollisionProp+1] = indexFrame
					end
				end
			end
			
			lineCount = lineCount+1
		end	
		
		file:close()
	end

end	

--- 
-- @description Save the current collision tiles state 
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:saveCollisionContentFile()</code></li>
-- </ul>
function TilesMap:saveCollisionContentFile()
	local finalPath = system.pathForFile( self.tilesCollisionInitFilePath, system.DocumentsDirectory)
	
	
	local objectsContent = ""
	local nbTilesX = self.nbTilesX 
	local nbTilesY = self.nbTilesY 
	
	for y = 0,nbTilesY -1 do 
		
		for x = 0,nbTilesX -1 do 
		
			if(x >= 0 and x < nbTilesX and y >= 0 and y < nbTilesY) then
				local indexTileInTabs = y * nbTilesX + x +1
				local indexFrameObject = self.tabTilesCollisionProp[indexTileInTabs]
				
				objectsContent = objectsContent.."i"..indexFrameObject
			end
			
		end
		
		objectsContent = objectsContent.."i\n"
	end
	
	local file = io.open( finalPath, "w+" )
	local result = file:write(objectsContent)
	file:close()
	if(not result) then 
		return "Write error"
	else
		return "Write Success"
	end 
end

--- 
-- @description Clean the current TilesMap Instance
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:clean()</li>
-- <li>tilesMapEngine = nil</code></li>
-- </ul>
function TilesMap:clean()

	if(self.Scroll) then 
		Runtime:removeEventListener("enterFrame",self.Scroll)
	end
	
	for i =1,#self.tabTiles do 
		local tile = self.tabTiles[i]
		tile:clean()
		tile = nil
	end
	
	self.displayGroup = nil
	self.tabTiles = nil
	self.xScroll =nil
	self.yScroll = nil
	
	self.nbTilesX =nil
	self.nbTilesY = nil
	
	self.texturesInitFilePath =nil
	self.objectsInitFilePath = nil
	self.tilesCollisionInitFilePath = nil
	
	self.tabFramesTextures = nil
	self.tabFramesObjects = nil
	self.tabTilesCollisionProp = nil
	self.tabObjectsInInteraction = nil
	
	self.xOrigin =nil
	self.yOrigin =nil
	
	self.tileWidth =nil
	self.tileHeight = nil
	
	self.textureSet = nil
	self.imageObjectSet = nil
	
	self.xTilesInView =nil
	self.yTilesInView = nil

	 
	self.prevX = nil
	self.prevY = nil
	self.currentX = nil
	self.currentY = nil
	
	
end


Tile = {}
Tile.__index = Tile

--ex params Tile
-- params = {
	--xDest,
	-- yDest,
	-- largeurTile,
	-- hauteurTile
	-- }
	
function Tile.create(tilesMapParent,xDest,yDest,density,bounce,friction,radius,collisionCategoryBits,collisionMaskBits)

	local tile = {}             -- our new object
	setmetatable(tile,Tile)  -- make Account handle lookup
	
	tile.tilesMapParent = tilesMapParent
	
	if(tilesMapParent.textureSet) then 
		--Create texture
		tile.texture = sprite.newSprite(tilesMapParent.textureSet)
		
		tile.texture.getInstance = function()
			return tile
		end
		
		tile.xOrigin = xDest 
		tile.yOrigin = yDest
		tile.texture.isVisible = false
		tile.texture:setReferencePoint(display.CenterReferencePoint)
		tile.texture.x = xDest
		tile.texture.y = yDest
		tilesMapParent.displayGroup:insert(tile.texture)
		
		-- Add Physic body
		if(tilesMapParent.isPhysicsEnabled == true) then 
			if(radius>0) then 
				physics.addBody(tile.texture,"static",{density = density,bounce = bounce,
											friction = friction,radius = radius,
											filter ={categoryBits = collisionCategoryBits,maskBits = collisionMaskBits}})
			else
				physics.addBody(tile.texture,"static",{density = density,bounce = bounce,friction = friction,
													filter ={categoryBits = collisionCategoryBits,maskBits = collisionMaskBits}})
			end
			

			tile.texture.angularDamping = 0
			tile.texture.angularVelocity = 0
			tile.texture.linearDamping = 0
			tile.texture.isFixedRotation = true
			tile.texture.isBodyActive = false
	
		end
	else
		tile.texture = display.newRect(xDest,yDest,tilesMapParent.tileWidth,tilesMapParent.tileHeight)
		
		tile.texture.getInstance = function()
			return tile
		end
		
		tile.xOrigin = xDest 
		tile.yOrigin = yDest
		tile.texture.isVisible = false
		tile.texture:setReferencePoint(display.CenterReferencePoint)
		tile.texture.x = xDest
		tile.texture.y = yDest
		tilesMapParent.displayGroup:insert(tile.texture)
		
		if(tilesMapParent.isPhysicsEnabled == true) then 
			-- Add Physic body
			if(radius>0) then 
				physics.addBody(tile.texture,"static",{density = density,bounce = bounce,
											friction = friction,radius = radius,
											filter ={categoryBits = collisionCategoryBits,maskBits = collisionMaskBits}})
			else
				physics.addBody(tile.texture,"static",{density = density,bounce = bounce,friction = friction,
											filter ={categoryBits = collisionCategoryBits,maskBits = collisionMaskBits}})
			end

			tile.texture.angularDamping = 0
			tile.texture.angularVelocity = 0
			tile.texture.linearDamping = 0
			tile.texture.isFixedRotation = true
			tile.texture.isBodyActive = false
		end
	end
	
	--Create ImageObject
	if(tilesMapParent.imageObjectSet) then 
		
		tile.imageObject = sprite.newSprite(tilesMapParent.imageObjectSet)
		tile.imageObject:setReferencePoint(display.CenterReferencePoint)
		tile.imageObject.isVisible = false
		tile.imageObject.x = xDest
		tile.imageObject.y = yDest 
		tilesMapParent.displayGroup:insert(tile.imageObject)
	end

	
	return tile
end

function Tile:move(xOffSet,yOffSet)
	if(self.texture.removeSelf) then 
		self.texture.x = self.xOrigin + xOffSet
		self.texture.y = self.yOrigin + yOffSet
		if(self.tilesMapParent.imageObjectSet) then 
			self.imageObject.x =  self.xOrigin + xOffSet
			self.imageObject.y =  self.yOrigin+ yOffSet
		end
	end
end

function Tile:clean()
	if(self.texture.removeSelf) then
		self.texture:removeSelf()
	end
	
	if(self.imageObject.removeSelf) then
		self.imageObject:removeSelf()
	end
end