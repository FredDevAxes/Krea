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
local json = require("json")
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
	tilesMap.xScale = 1
	tilesMap.yScale = 1
	tilesMap.events = params.events
	tilesMap.dynamicScale = params.dynamicScale
	tilesMap.scaleSuffix = params.scaleSuffix
	--Check that all the events listener have been declared
	for i =1, #tilesMap.events do 
		local event = tilesMap.events[i]
		if(event.listener == nil)then 
			print("WARNING: The listener of the event "..event.name.." has not been declared in the scene Lua file!")
		end
	end	
	--Init tilesmap ressource file 
	local path = string.lower(tilesMap.name).."config.json"
	local configContent = tilesMap:getFileContent(path)
	tilesMap.infos = json.decode(configContent)
	
	tilesMap.isPhysicsEnabled = tilesMap.infos.isPhysicsEnabled
	tilesMap.nbTilesX = tilesMap.infos.NbColumns
	tilesMap.nbTilesY = tilesMap.infos.NbLines
	tilesMap.tileWidth = tilesMap.infos.TilesWidth
	tilesMap.tileHeight = tilesMap.infos.TilesHeight

	if(tilesMap.infos.TextureCount>0 or tilesMap.infos.TextureSequenceCount>0) then 
		local tabSpriteSet = {}
		
		if(tilesMap.infos.TextureCount>0) then 
			-- TREAT TEXTURE SHEETS
			for i=0,tilesMap.infos.TextureSheetCount -1 do
				local indexInTabSpriteSet = #tabSpriteSet +1
				tabSpriteSet[indexInTabSpriteSet] = {}
				local texturesData = require(tilesMap.name.."textsheet"..i.."data"..tilesMap.scaleSuffix).getSpriteSheetData()
				local pngName = tilesMap.name.."textsheet"..i..tilesMap.scaleSuffix..".png"
				local texturesSheet = sprite.newSpriteSheetFromData( pngName, texturesData )
				tabSpriteSet[indexInTabSpriteSet].sheet = texturesSheet
				
				local frames = {}
				for j =1,tilesMap.infos.TextureCountBySheet[i+1] do 
					table.insert(frames,j)
				end
				tabSpriteSet[indexInTabSpriteSet].frames = frames
			end
		end
		
		if(tilesMap.infos.TextureSequenceCount>0) then 
			-- TREAT TEXTURE SEQUENCE SHEETS
			for i=0,tilesMap.infos.TextureSequenceSheetCount -1 do
				local indexInTabSpriteSet = #tabSpriteSet +1
				tabSpriteSet[indexInTabSpriteSet] = {}
				local texturesData = require(tilesMap.name.."textsequencessheet"..i.."data"..tilesMap.scaleSuffix).getSpriteSheetData()
				local pngName = tilesMap.name.."textsequencessheet"..i..tilesMap.scaleSuffix..".png"
				local texturesSheet = sprite.newSpriteSheetFromData( pngName, texturesData )
				tabSpriteSet[indexInTabSpriteSet].sheet = texturesSheet
				
				local frames = {}
				for j =1,tilesMap.infos.TextureSequenceCountBySheet[i+1] do 
					table.insert(frames,j)
				end
				tabSpriteSet[indexInTabSpriteSet].frames = frames
			end
		end
		
		tilesMap.textureSet = sprite.newSpriteMultiSet(tabSpriteSet)
		
		-- Create Texture Sequences
		for i = 1, #tilesMap.infos.TextureSequences do 
			local seq = tilesMap.infos.TextureSequences[i]
			sprite.add(tilesMap.textureSet,seq.Name,seq.StartAtFrame,seq.FrameCount,seq.Lenght,seq.Iteration)
		end
	end

	if(tilesMap.infos.ObjectCount>0  or tilesMap.infos.ObjectSequenceCount>0) then 
		local tabSpriteSet = {}
		
		if(tilesMap.infos.ObjectCount>0) then 
			-- TREAT OBJECT SHEETS
			for i=0,tilesMap.infos.ObjectSheetCount -1 do
				local indexInTabSpriteSet = #tabSpriteSet +1
				tabSpriteSet[indexInTabSpriteSet] = {}
				local objectsData = require(tilesMap.name.."objsheet"..i.."data"..tilesMap.scaleSuffix).getSpriteSheetData()
				local pngName = tilesMap.name.."objsheet"..i..tilesMap.scaleSuffix..".png"
				local objectSheet = sprite.newSpriteSheetFromData( pngName, objectsData )
				tabSpriteSet[indexInTabSpriteSet].sheet = objectSheet

				
				local frames = {}
				for j =1,tilesMap.infos.ObjectCountBySheet[i+1] do 
					table.insert(frames,j)
				end
				tabSpriteSet[indexInTabSpriteSet].frames = frames
			end
		end
		
		if(tilesMap.infos.ObjectSequenceCount>0) then 
			-- TREAT OBJECT SEQUENCE SHEETS
			for i=0,tilesMap.infos.ObjectSequenceSheetCount -1 do
				local indexInTabSpriteSet = #tabSpriteSet +1
				tabSpriteSet[indexInTabSpriteSet] = {}
				local objectsData = require(tilesMap.name.."objsequencessheet"..i.."data"..tilesMap.scaleSuffix).getSpriteSheetData()
				local pngName = tilesMap.name.."objsequencessheet"..i..tilesMap.scaleSuffix..".png"
				local objectSheet = sprite.newSpriteSheetFromData( pngName, objectsData )
				tabSpriteSet[indexInTabSpriteSet].sheet = objectSheet

				
				local frames = {}
				for j =1,tilesMap.infos.ObjectCountBySheet[i+1] do 
					table.insert(frames,j)
				end
				tabSpriteSet[indexInTabSpriteSet].frames = frames
			end
		end
		
		tilesMap.imageObjectSet = sprite.newSpriteMultiSet(tabSpriteSet)
		
		-- Create Object Sequences
		for i = 1, #tilesMap.infos.ObjectSequences do 
			local seq = tilesMap.infos.ObjectSequences[i]
			sprite.add(tilesMap.imageObjectSet,seq.Name,seq.StartAtFrame,seq.FrameCount,seq.Lenght,seq.Iteration)
		end
	end 
	
	
	tilesMap.xScroll = params.xScroll - tilesMap.tileWidth *2
	tilesMap.yScroll = params.yScroll - tilesMap.tileHeight *2
	
	
	
	tilesMap.isPaused = true
	tilesMap.tabTiles = {}
	
	tilesMap.tabObjectsInInteraction = {}
	
	-- tilesMap.xOrigin = params.xPos + tilesMap.tileWidth
	-- tilesMap.yOrigin = params.yPos + tilesMap.tileHeight
	
	
	tilesMap:loadContentFiles()
	tilesMap.xTilesInView = floor((contentWidth -screenOriginX) / tilesMap.tileWidth) +5
	tilesMap.yTilesInView = floor((contentHeight  -screenOriginY) / tilesMap.tileHeight)+5

	tilesMap.prevX = tilesMap.displayGroup.x
	tilesMap.prevY = tilesMap.displayGroup.y
	tilesMap.currentX = tilesMap.displayGroup.x
	tilesMap.currentY = tilesMap.displayGroup.y
	
	
	tilesMap:populate()
	tilesMap:updateDisplay()
	
	if(tilesMap.infos.IsPathFindingEnabled == true) then 
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
				tilesMap:updateDisplay()
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
	self:updateDisplay()

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

	self:loadFramesIndexTextures()
	self:loadFramesIndexImageObjects()
	self:loadTilesIndexTextureSequences()
	self:loadTilesIndexImageObjectSequences()
	self:loadTilesIndexEvents()
	
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

	local friction = self.infos.Friction
	local density = self.infos.Density
	local bounce = self.infos.Bounce
	local radius = self.infos.Radius
	local collisionCatBits = self.infos.CollisionCategoryBits
	local collisionMaskBits = self.infos.CollisionMaskBits
	local halfTileWidth = self.tileWidth*0.5
	local halfTileHeight = self.tileHeight*0.5
	for y = 0, self.yTilesInView -1 do
        for x = 0, self.xTilesInView -1 do
			
			--Set tile location
			
			local xDest = (x * self.tileWidth) + halfTileWidth -- (halfTileWidth *(1/self.dynamicScale))
			local yDest = (y * self.tileHeight)+ halfTileHeight -- (halfTileHeight*(1/self.dynamicScale))
			
			local tile = Tile.create(self,xDest,yDest,self.dynamicScale,density,bounce,friction,radius,collisionCatBits,collisionMaskBits)
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
  local currentScrollX = self.xScroll
  local currentScrollY = self.yScroll
  
 
  local diffX =  - (currentScrollX % self.tileWidth)
  local diffY =  - (currentScrollY % self.tileHeight)

  local scrollX = floor((currentScrollX ) / self.tileWidth) 
  local scrollY = floor((currentScrollY ) / self.tileHeight)

  local xMove = currentScrollX+diffX -- self.xOrigin
  local yMove = currentScrollY+diffY -- self.yOrigin
  
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
			if(indexFrameTexture > -1) then 
				t.texture:prepare("default")		
				t.texture.currentFrame = indexFrameTexture+1
			
				t.texture.isVisible = true
			else
				local indexTextureSequence = self.tabTilesTextureSequences[indexTileInTabs]
				if(indexTextureSequence > -1) then 
					local seq = self.infos.TextureSequences[indexTextureSequence+1]
					if(t.texture.sequence ~= seq.Name) then 
						t.texture:prepare(seq.Name)
						t.texture.currentFrame = 1
						t.texture:play()
					end
					t.texture.isVisible = true
				else
					t.texture.isVisible = false
					t.texture:prepare("default")	
				end
			end
		end
		
		
		--Show object if exists
		if(self.imageObjectSet) then 
			local indexFrameObject = self.tabFramesObjects[indexTileInTabs]
			if(indexFrameObject >-1) then 
				
				t.imageObject:prepare("default")	
				t.imageObject.currentFrame = indexFrameObject +1
				t.imageObject.isVisible = true
				
			else
				local indexObjectSequence = self.tabTilesObjectSequences[indexTileInTabs]
				if(indexObjectSequence > -1) then 
					
					local seq = self.infos.ObjectSequences[indexObjectSequence+1]
					if(t.imageObject.sequence ~= seq.Name) then 
						t.imageObject:prepare(seq.Name)
						t.imageObject.currentFrame = 1
						t.imageObject:play()
					end
					t.imageObject.isVisible = true
				else
					t.imageObject.isVisible = false
					t.imageObject:prepare("default")	
				end
			end
		end
		
		
		-- Get the collisionning for this tile
		if(self.isPhysicsEnabled == true) then 
			if(self.isPaused == false) then 
				local isCrossable = self.tabTilesCollisionProp[indexTileInTabs]
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
		
		-- Get the event for this tile
		local indexEvent = self.tabTilesEvents[indexTileInTabs]
		if(indexEvent >-1) then 
			
			local eventToAdd = self.events[indexEvent+1]
			
			if(eventToAdd) then 
				if(eventToAdd ~= t.currentEvent) then 
					-- ADD THE EVENT
					t:addEventListener(eventToAdd)
				end
			else
				--Remove current event
				t:removeCurrentEventListener()
			end
		else
			t:removeCurrentEventListener()
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
		
		if(t.currentEvent) then 
			if(t.currentEvent.listener)then 
				if(t.texture) then
						if(t.currentEvent.type == "touch") then 
							t.texture:removeEventListener(t.currentEvent.type,t.currentEvent.listener)
						elseif(t.currentEvent.type == "collision") then
							t.texture:removeEventListener(t.currentEvent.type,t.texture)
							t.texture.collision = nil
						elseif(t.currentEvent.type == "preCollision") then
							t.texture:removeEventListener(t.currentEvent.type,t.texture)
							t.texture.preCollision = nil
						elseif(t.currentEvent.type == "postCollision") then
							t.texture:removeEventListener(t.currentEvent.type,t.texture)
							t.texture.postCollision = nil
						end
					elseif(t.imageObject) then 
						if(t.currentEvent.type == "touch") then 
							t.imageObject:removeEventListener(t.currentEvent.type,t.currentEvent.listener)
						elseif(t.currentEvent.type == "collision") then
							t.imageObject:removeEventListener(t.currentEvent.type,t.imageObject)
							t.imageObject.collision = nil
						elseif(t.currentEvent.type == "preCollision") then
							t.imageObject:removeEventListener(t.currentEvent.type,t.imageObject)
							t.imageObject.preCollision = nil
						elseif(t.currentEvent.type == "postCollision") then
							t.imageObject:removeEventListener(t.currentEvent.type,t.imageObject)
							t.imageObject.postCollision = nil
						end
					end	
			end
			t.currentEvent = nil
		end
			
	end
end

--- 
-- @description Set the auto scroll active. Scroll when the displayGroupParent location is changed
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:setAutoScrollActive()</code></li>
-- </ul>
function TilesMap:setAutoScrollActive()
	self:setDragInactive()

	if(not self.Scroll) then 
		local parent = self.displayGroup
		self.currentX = parent.x
		self.currentY = parent.y
		
		self.prevX = self.currentX 
		self.prevY = self.currentY 
		
		self.Scroll = function (event)
			self.currentX = parent.x
			self.currentY = parent.y
			
			if(self.prevX ~= self.currentX or self.prevY ~= self.currentY) then 
			
				local xOffSet = self.prevX - self.currentX
				local yOffSet = self.prevY - self.currentY
				self.xScroll = self.xScroll + xOffSet
				self.yScroll = self.yScroll + yOffSet
						
				self:updateDisplay()
			end
		
			self.prevX = self.currentX 
			self.prevY = self.currentY 
			
			
		end
		
		
		Runtime:addEventListener("enterFrame",self.Scroll)
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
			self.Scroll = nil
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

function TilesMap:getFileContent( filename, base )

	-- set default base dir if none specified
	if not base then base = system.ResourceDirectory; end
	-- create a file path for corona i/o
	local path = system.pathForFile( filename, base )

	-- will hold contents of file
	local contents

	-- io.open opens a file at path. returns nil if no file found
	local file = io.open( path, "r" )
	if file then
	-- read all contents of file into a string
	contents = file:read( "*a" )
	io.close( file )	-- close the file after using it
	else
		print("file ".. filename.." not found")
	end 

	return contents
end
--- 
-- @description Load the Texture tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadFramesIndexTextures()</code></li>
-- </ul>
function TilesMap:loadFramesIndexTextures()
	
	self.tabFramesTextures = nil
	local filename = string.lower(self.name).."textures.json"
	local fileContent = self:getFileContent(filename)
	self.tabFramesTextures = json.decode(fileContent)

end

--- 
-- @description Load the Texture Sequence tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadTilesTextureSequences()</code></li>
-- </ul>
function TilesMap:loadTilesIndexTextureSequences()
	
	self.tabTilesTextureSequences = nil
	local filename = string.lower(self.name).."texturesequences.json"
	local fileContent = self:getFileContent(filename)
	self.tabTilesTextureSequences = json.decode(fileContent)
	
end

--- 
-- @description Load the object tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadFramesIndexImageObjects()</code></li>
-- </ul>
function TilesMap:loadFramesIndexImageObjects()

	self.tabFramesObjects = nil
	
	local filename = string.lower(self.name).."objects.json"
	local fileContent = self:getFileContent(filename)
	self.tabFramesObjects = json.decode(fileContent)
end	

--- 
-- @description Load the object sequence tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadFramesIndexImageObjectSequences()</code></li>
-- </ul>
function TilesMap:loadTilesIndexImageObjectSequences()

	self.tabTilesObjectSequences = nil
	
	local filename = string.lower(self.name).."objectsequences.json"
	local fileContent = self:getFileContent(filename)
	self.tabTilesObjectSequences = json.decode(fileContent)
end	

--- 
-- @description Save the object tiles state
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:saveFramesObjectsTextures()</code></li>
-- </ul>
function TilesMap:saveFramesObjectsTextures()
	local finalPath = system.pathForFile( self.objectsInitFilePath, system.ResourceDirectory)
	
	
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
	
	local filename = string.lower(self.name).."collisions.json"
	local fileContent = self:getFileContent(filename)
	self.tabTilesCollisionProp = json.decode(fileContent)
	
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
-- @description Load the object event tiles state from file
-- @usage Usage:
-- <ul>
-- <li><code>tilesMapEngine:loadTilesIndexEvents()</code></li>
-- </ul>
function TilesMap:loadTilesIndexEvents()

	self.tabTilesEvents = nil
	
	local filename = string.lower(self.name).."events.json"
	local fileContent = self:getFileContent(filename)
	self.tabTilesEvents = json.decode(fileContent)
end	

function TilesMap:scale(xScale,yScale)

	self.xScale = xScale
	self.yScale = yScale
	
	self:updateDisplay()
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
	
function Tile.create(tilesMapParent,xDest,yDest,scale,density,bounce,friction,radius,collisionCategoryBits,collisionMaskBits)

	local tile = {}             -- our new object
	setmetatable(tile,Tile)  -- make Account handle lookup
	
	tile.tilesMapParent = tilesMapParent
	
	if(tilesMapParent.textureSet) then 
		--Create texture
		tile.texture = sprite.newSprite(tilesMapParent.textureSet)
		tile.texture:scale(scale,scale)
		tile.texture.getInstance = function()
			return tile
		end
		
		xDest = xDest 
		yDest = yDest 
		
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
				local halfTileWidth = tile.texture.contentWidth /2
				local halfTileHeight = tile.texture.contentHeight /2
				local physicsShape = {-halfTileWidth,-halfTileHeight,
										halfTileWidth,-halfTileHeight,
										halfTileWidth,halfTileHeight
										,-halfTileWidth,halfTileHeight}
				physics.addBody(tile.texture,"static",{density = density,bounce = bounce,friction = friction,
													filter ={categoryBits = collisionCategoryBits,maskBits = collisionMaskBits},shape = physicsShape})
			end
			

			tile.texture.angularDamping = 0
			tile.texture.angularVelocity = 0
			tile.texture.linearDamping = 0
			tile.texture.isFixedRotation = true
			tile.texture.isBodyActive = false
	
		end
	else
		tile.texture = display.newRect(xDest,yDest,tilesMapParent.tileWidth,tilesMapParent.tileHeight)
		tile.texture:scale(scale,scale)
		tile.texture.getInstance = function()
			return tile
		end
		
		xDest = xDest 
		yDest = yDest 
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
				local halfTileWidth = tile.texture.contentWidth /2
				local halfTileHeight = tile.texture.contentHeight /2
				local physicsShape = {-halfTileWidth,-halfTileHeight,
										halfTileWidth,-halfTileHeight,
										halfTileWidth,halfTileHeight
										,-halfTileWidth,halfTileHeight}
				physics.addBody(tile.texture,"static",{density = density,bounce = bounce,friction = friction,
													filter ={categoryBits = collisionCategoryBits,maskBits = collisionMaskBits},shape = physicsShape})
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
		tile.imageObject:scale(scale,scale)
		
				
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
		self.texture.x = (self.xOrigin + xOffSet) * self.tilesMapParent.xScale
		self.texture.y = (self.yOrigin + yOffSet) * self.tilesMapParent.yScale
		if(self.tilesMapParent.imageObjectSet) then 
			self.imageObject.x =  (self.xOrigin + xOffSet)* self.tilesMapParent.xScale
			self.imageObject.y =  (self.yOrigin+ yOffSet)* self.tilesMapParent.yScale
		end
	end
end

function Tile:addEventListener(event)
	self:removeCurrentEventListener()
	
	self.currentEvent = event
					
	if(self.currentEvent.listener)then 
		if(self.texture) then
			if(self.currentEvent.type == "touch") then 
				self.texture:addEventListener(self.currentEvent.type,self.currentEvent.listener)
			elseif(self.currentEvent.type == "collision") then
				self.texture.collision = self.currentEvent.listener
				self.texture:addEventListener(self.currentEvent.type,self.texture)
			elseif(self.currentEvent.type == "preCollision") then
				self.texture.preCollision = self.currentEvent.listener
				self.texture:addEventListener(self.currentEvent.type,self.texture)
			elseif(self.currentEvent.type == "postCollision") then
				self.texture.postCollision = self.currentEvent.listener
				self.texture:addEventListener(self.currentEvent.type,self.texture)
			end
		elseif(self.imageObject) then 
			if(self.currentEvent.type == "touch") then 
				self.imageObject:addEventListener(self.currentEvent.type,self.currentEvent.listener)
			elseif(self.currentEvent.type == "collision") then
				self.imageObject.collision = self.currentEvent.listener
				self.imageObject:addEventListener(self.currentEvent.type,self.imageObject)
			elseif(self.currentEvent.type == "preCollision") then
				self.imageObject.preCollision = self.currentEvent.listener
				self.imageObject:addEventListener(self.currentEvent.type,self.imageObject)
			elseif(self.currentEvent.type == "postCollision") then
				self.imageObject.postCollision = self.currentEvent.listener
				self.imageObject:addEventListener(self.currentEvent.type,self.imageObject)
			end
		end	
	end
end

function Tile:removeCurrentEventListener()
	if(self.currentEvent) then 
		if(self.currentEvent.listener)then 
			if(self.texture) then
				if(self.currentEvent.type == "touch") then 
					self.texture:removeEventListener(self.currentEvent.type,self.currentEvent.listener)
				elseif(self.currentEvent.type == "collision") then
					self.texture.collision = self.currentEvent.listener
					self.texture:removeEventListener(self.currentEvent.type,self.texture)
				elseif(self.currentEvent.type == "preCollision") then
					self.texture.preCollision = self.currentEvent.listener
					self.texture:removeEventListener(self.currentEvent.type,self.texture)
				elseif(self.currentEvent.type == "postCollision") then
					self.texture.postCollision = self.currentEvent.listener
					self.texture:removeEventListener(self.currentEvent.type,self.texture)
				end
			elseif(self.imageObject) then 
				if(self.currentEvent.type == "touch") then 
					self.imageObject:removeEventListener(self.currentEvent.type,self.currentEvent.listener)
				elseif(self.currentEvent.type == "collision") then
					self.imageObject.collision = self.currentEvent.listener
					self.imageObject:removeEventListener(self.currentEvent.type,self.imageObject)
				elseif(self.currentEvent.type == "preCollision") then
					self.imageObject.preCollision = self.currentEvent.listener
					self.imageObject:removeEventListener(self.currentEvent.type,self.imageObject)
				elseif(self.currentEvent.type == "postCollision") then
					self.imageObject.postCollision = self.currentEvent.listener
					self.imageObject:removeEventListener(self.currentEvent.type,self.imageObject)
				end
			end	
		end
		self.currentEvent = nil
	end
end

function Tile:clean()
	if(self.texture.removeSelf) then
		self.texture:removeSelf()
	end
	self.texture = nil
	
	if(self.imageObject.removeSelf) then
		self.imageObject:removeSelf()
	end
	
	self.imageObject = nil
end