--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description FStar is a module providing an algorithm to find a possible path through the tiles of the module TilesMap. 
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("fstar", package.seeall)

--- 
-- Members of the FStar Instance
-- @class table
-- @name Fields
-- @field tilesmap The tiles map on which the path finding will calculate the paths

FStar = {}
FStar.__index = FStar


--- 
-- @description Create a new FStar Engine Instance working a TilesMapInstance
-- @return Return a FStar Engine Instance
-- @param params Lua Table containing the params
-- @see tilesmap
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "name"
-- <br>params.tilesMapParent = tilesmap1Instance</li>
-- <li> local fstarEngine = require("fstar").FStar.create(params)</code></li>

function FStar.create(params)
	
	--Init attributes of instance
	local fstar = {}           
	setmetatable(fstar,FStar)
	
	fstar.tilesMap = params.tilesMapParent
	
	return fstar
end

function FStar:setup()
	self.paths = {}
	self.tilesFailed = {}
end

-- Function returns a table of moves allowed into the tiles map (UP,DOWN,LEFT,RIGHT)
function FStar:getMovesAllowed(tileFrom,tileDest)

	local tabMoves = {}
	
	if(self.tilesMap) then 
		local tilesMap = self.tilesMap
		
		local xIndex = tileFrom.xIndex
		local yIndex = tileFrom.yIndex 
		
		--Treat on X
		local previousTileX = xIndex -1 
		if(previousTileX >=0 and previousTileX <tilesMap.xTilesInView ) then 
			local tile = tilesMap.tabTiles[tilesMap.xTilesInView * yIndex + previousTileX + 1]
			if(tile.texture.isBodyActive == false) then 
				table.insert(tabMoves,"LEFT")
			end
		end
		
		local nextTileY = yIndex +1 
		if(nextTileY>=0 and nextTileY <tilesMap.yTilesInView) then 
			
			local tile = tilesMap.tabTiles[tilesMap.xTilesInView * nextTileY + xIndex + 1]
			if(tile.texture.isBodyActive == false) then 
				table.insert(tabMoves,"DOWN")
			end
		
		end
		
		--Treat on Y
		local previousTileY = yIndex -1 
		if(previousTileY >=0 and previousTileY < tilesMap.yTilesInView) then 
			local tile = tilesMap.tabTiles[tilesMap.xTilesInView * previousTileY + xIndex  + 1]
			if(tile.texture.isBodyActive == false) then 
				table.insert(tabMoves,"UP")
			end
		end
		
		
		
		
		
		local nextTileX = xIndex +1 
		if(nextTileX >=0 and nextTileX <tilesMap.xTilesInView) then 
			
			local tile = tilesMap.tabTiles[tilesMap.xTilesInView * yIndex + nextTileX + 1]
			if(tile.texture.isBodyActive == false) then 
				table.insert(tabMoves,"RIGHT")
			end
		
		end
		
		
		-- Set priorities
		local movesOnX = {}
		local movesOnY = {}
		local diffX = math.abs(tileDest.xIndex - tileFrom.xIndex)
		local diffY = math.abs(tileDest.yIndex - tileFrom.yIndex)
		
		local isLeftAllowed = self:checkIfTabContainsValue(tabMoves,"LEFT")
		local isRightAllowed = self:checkIfTabContainsValue(tabMoves,"RIGHT")
		local isUpAllowed = self:checkIfTabContainsValue(tabMoves,"UP")
		local isDownAllowed = self:checkIfTabContainsValue(tabMoves,"DOWN")
		
		local finaltab = {}
		if(tileDest.xIndex >= tileFrom.xIndex and tileDest.yIndex >= tileFrom.yIndex) then 
			if(diffX >=diffY) then 
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
				if(isUpAllowed) then table.insert(finaltab,"UP") end
			else
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
			end
			
		elseif(tileDest.xIndex >= tileFrom.xIndex and tileDest.yIndex <=tileFrom.yIndex) then
			if(diffX >=diffY) then 
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				
			else
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
			end
			
		elseif(tileDest.xIndex <= tileFrom.xIndex and tileDest.yIndex >= tileFrom.yIndex) then
			if(diffX >=diffY) then 
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				
			else
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
			end
		elseif(tileDest.xIndex <= tileFrom.xIndex and tileDest.yIndex <= tileFrom.yIndex) then
			if(diffX >=diffY) then 
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				
			else
				if(isUpAllowed) then table.insert(finaltab,"UP") end
				if(isLeftAllowed) then table.insert(finaltab,"LEFT") end
				if(isDownAllowed) then table.insert(finaltab,"DOWN") end
				if(isRightAllowed) then table.insert(finaltab,"RIGHT") end
			end
		end
		
		
		return finaltab
	end
	
	return tabMoves
end

function FStar:checkIfTabContainsValue(tab,value)
	for i = 1, #tab do 
	
		local val = tab[i]
		if(val == value) then 
			return true
		end
	end
	
	return false
end

--- 
-- @description Find a path between two tiles through a physic based tilesmap
-- @return Return a table of tile index if path found, nil otherwise
-- @usage Usage:
-- <ul>
-- <li> <code>local tilesFrom = fstarEngine.tilesMap.tabTiles[3]
-- <br>local tilesDest = fstarEngine.tilesMap.tabTiles[10]</li>
-- <li>local tabPathIndex = fstarEngine:findPath(tileFrom,tileDest)</li>
-- </ul>

function FStar:findPath(tileFrom,tileDest)
	self:setup()
	self:calcPath(nil,tileFrom,tileDest)
	
	--Get the path the shortest
	local indexPathShortest = -1
	local shortest = 1000

	
	for j = 1,#self.paths do 
		local tabPath = self.paths[j]
		if(tabPath~=nil) then 
			 if(#tabPath < shortest) then 
				indexPathShortest = j
				shortest = #tabPath
			 end
			 
		end
	end
	
	if(indexPathShortest~= -1) then 
		local tab = self.paths[indexPathShortest]
		return tab
	else
		print("NO PATH")
		return nil
	end
end

function FStar:calcPath(currentTabPath,tileFrom,tileDest)

	if(currentTabPath == nil) then 
		currentTabPath = {}
	end
	local hasFoundPath = false
	
	local isFailed = self:doesTileAlreadyFailed(tileFrom)
	if(isFailed == false) then 
		local isAlreadyIn = self:checkIfTileAlreadyInPath(currentTabPath,tileFrom)

		if(isAlreadyIn == false) then 

			table.insert(currentTabPath,tileFrom)
			local indexTile = #currentTabPath
			
			
			if(tileFrom ~= tileDest) then 
				local tabMoves = self:getMovesAllowed(tileFrom,tileDest)
				for i = 1, #tabMoves do 
					
					local move = tabMoves[i]
					-- if go UP
					if(move == "UP") then 
						local yIndex = tileFrom.yIndex -1
						if(yIndex >=0 and yIndex < self.tilesMap.yTilesInView) then 

							local tileUp = self.tilesMap.tabTiles[self.tilesMap.xTilesInView * yIndex + tileFrom.xIndex + 1]
							local res = self:calcPath(currentTabPath,tileUp,tileDest)
							if(res == true) then 
								hasFoundPath = true
							elseif(res == false) then 
								table.insert(self.tilesFailed,tileUp)
							end
							
						end
					elseif(move == "DOWN") then 
						local yIndex = tileFrom.yIndex +1
						if(yIndex >=0 and yIndex < self.tilesMap.yTilesInView) then 

							local tileDown = self.tilesMap.tabTiles[self.tilesMap.xTilesInView * yIndex + tileFrom.xIndex + 1]
							local res = self:calcPath(currentTabPath,tileDown,tileDest)
							if(res == true) then 
								hasFoundPath = true
							elseif(res == false) then 
								table.insert(self.tilesFailed,tileDown)
							end
							
							
						end
					elseif(move == "RIGHT") then 
						local xIndex = tileFrom.xIndex +1
						if(xIndex >=0 and xIndex < self.tilesMap.xTilesInView) then 

							local tileRight = self.tilesMap.tabTiles[self.tilesMap.xTilesInView * tileFrom.yIndex + xIndex + 1]
							local res = self:calcPath(currentTabPath,tileRight,tileDest)
							if(res == true) then 
								hasFoundPath = true
							elseif(res == false) then 
								table.insert(self.tilesFailed,tileRight)
							end
							
						end
					elseif(move == "LEFT") then 
						local xIndex = tileFrom.xIndex -1
						if(xIndex >=0 and xIndex < self.tilesMap.xTilesInView) then 

							local tileLeft = self.tilesMap.tabTiles[self.tilesMap.xTilesInView * tileFrom.yIndex + xIndex + 1]
							local res = self:calcPath(currentTabPath,tileLeft,tileDest)
							if(res == true) then 
								hasFoundPath = true
							elseif(res == false) then 
								table.insert(self.tilesFailed,tileLeft)
							end
							
						end
					end

				end
				
			else
				local pathCopy = table.copy(currentTabPath)
				table.insert(self.paths,pathCopy)
				hasFoundPath = true
			end
			
			table.remove(currentTabPath,indexTile)

			return hasFoundPath
		else
			return nil
		end
	else
		return nil
	end
	
end

function FStar:checkIfTileAlreadyInPath(tabPaths,tile)
	
	for i = 1, #tabPaths do 
	
		local path = tabPaths[i]
		if(path == tile) then 
			return true
		end
	end
	
	return false
end

function FStar:doesTileAlreadyFailed(tile)
	
	for i = 1, #self.tilesFailed do 
	
		local t = self.tilesFailed[i]
		if(t == tile) then 
			return true
		end
	end
	
	return false
end
