--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description perfanalyser is needed by Krea 
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("perfanalyser", package.seeall)

local graphInstance = require("graph")

local prevTime = 0;
local maxSavedFps = 30;
 
local screenOriginX = display.screenOriginX
local screenOriginY = display.screenOriginY
local contentWidth = display.contentWidth
local contentHeight = display.contentHeight

local halfContentWidth = contentWidth * 0.5
local halfContentHeight = contentHeight * 0.5
local mFloor = math.floor

local function minElement(table)
        local min = 10000;
        for i = 1, #table do
                if(table[i] < min) then min = table[i]; end
        end
        return min;
end

local function maxElement(table)
        local max = 0;
        for i = 1, #table do
                if(table[i] > max) then max = table[i]; end
        end
        return max;
end

local function averageElement(table)
        local average = 0
		local total = 0
		
        for i = 1, #table do
              total = total +table[i]
        end
		
		average = total / #table
        return mFloor(average);
end

PerfAnalyser = {}
PerfAnalyser.__index = PerfAnalyser


function PerfAnalyser.create(params)
	
	--Init attributes of instance
	local perfAnalyser = {}           
	setmetatable(perfAnalyser,PerfAnalyser)
	
	
	perfAnalyser.group = params.group
	
	local g = graphics.newGradient({0,255,0},{255,0,0},"up")
	
	----------------------- FPS GRAPH -----------------------------------------------------------------------------------------
	
	---- Back FPS and Graph
	perfAnalyser.backgroundGraphFPS = display.newRect( screenOriginX,screenOriginY,
					contentWidth - screenOriginX *2, (contentHeight -screenOriginY *2)/10)
	perfAnalyser.group:insert(perfAnalyser.backgroundGraphFPS)
	perfAnalyser.backgroundGraphFPS:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.backgroundGraphFPS.x = screenOriginX
	perfAnalyser.backgroundGraphFPS.y = screenOriginY
	perfAnalyser.backgroundGraphFPS:setFillColor(g)
	perfAnalyser.backgroundGraphFPS.alpha = 0.9
	
	local paramsFPSGraph = {}
	paramsFPSGraph.minValue = 0
	paramsFPSGraph.maxValue = 30

	local surfaceRectFPSInstance = {object = perfAnalyser.backgroundGraphFPS, displayGroupParent = perfAnalyser.group}
	paramsFPSGraph.surfaceObject = surfaceRectFPSInstance
	paramsFPSGraph.maxValuesInHistoric = perfAnalyser.backgroundGraphFPS.contentWidth/3
	paramsFPSGraph.refreshInterval = 10
	paramsFPSGraph.lineColor = {255,255,0}
	perfAnalyser.graphFPS = graphInstance.Graph.create(paramsFPSGraph)
	
	
	---- Back FPS TEXTS
	perfAnalyser.backgroundTextFPS = display.newRect( screenOriginX,screenOriginY + perfAnalyser.backgroundGraphFPS.contentHeight,
					contentWidth - screenOriginX *2, (contentHeight -screenOriginY *2)/30)
	perfAnalyser.group:insert(perfAnalyser.backgroundTextFPS)
	perfAnalyser.backgroundTextFPS:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.backgroundTextFPS.x = screenOriginX
	perfAnalyser.backgroundTextFPS.y = screenOriginY + perfAnalyser.backgroundGraphFPS.contentHeight
	perfAnalyser.backgroundTextFPS:setFillColor(0,0,0)
	perfAnalyser.backgroundTextFPS.alpha = 0.9
	
	perfAnalyser.textFPS = display.newText("0",contentWidth * 0.5, perfAnalyser.backgroundTextFPS.y, "Arial", 14)
	perfAnalyser.textFPS:setTextColor(255,255,255)
	perfAnalyser.textFPS:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.textFPS.x = contentWidth * 0.5
	perfAnalyser.textFPS.y =  perfAnalyser.backgroundTextFPS.y
	perfAnalyser.group:insert(perfAnalyser.textFPS)
	
	
	----------------------- TEXTURE MEMORY GRAPH -----------------------------------------------------------------------------------------
	---- Back Texture and Graph
	perfAnalyser.backgroundGraphTextureMemory = display.newRect( screenOriginX,perfAnalyser.backgroundTextFPS.y + perfAnalyser.backgroundTextFPS.contentHeight,
					contentWidth/2 - screenOriginX, (contentHeight -screenOriginY *2)/15)
	perfAnalyser.group:insert(perfAnalyser.backgroundGraphTextureMemory)
	perfAnalyser.backgroundGraphTextureMemory:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.backgroundGraphTextureMemory.x = screenOriginX
	perfAnalyser.backgroundGraphTextureMemory.y = perfAnalyser.backgroundTextFPS.y + perfAnalyser.backgroundTextFPS.contentHeight
	perfAnalyser.backgroundGraphTextureMemory:setFillColor(g)
	perfAnalyser.backgroundGraphTextureMemory.alpha = 0.9
	
	local paramsTextureMemoryGraph = {}
	paramsTextureMemoryGraph.minValue = 0
	paramsTextureMemoryGraph.maxValue = 50

	local surfaceRectTextureMemoryInstance = {object = perfAnalyser.backgroundGraphTextureMemory, displayGroupParent = perfAnalyser.group}
	paramsTextureMemoryGraph.surfaceObject = surfaceRectTextureMemoryInstance
	paramsTextureMemoryGraph.maxValuesInHistoric = perfAnalyser.backgroundGraphTextureMemory.contentWidth/3
	paramsTextureMemoryGraph.refreshInterval = 10
	paramsTextureMemoryGraph.lineColor = {0,0,255}
	perfAnalyser.graphTextureMemory = graphInstance.Graph.create(paramsTextureMemoryGraph)
	
	
	---- Back TextureMemory TEXTS
	perfAnalyser.backgroundTextTextureMemory = display.newRect( screenOriginX,perfAnalyser.backgroundGraphTextureMemory.y + perfAnalyser.backgroundGraphTextureMemory.contentHeight,
					contentWidth/2 - screenOriginX, (contentHeight -screenOriginY *2)/30)
	perfAnalyser.group:insert(perfAnalyser.backgroundTextTextureMemory)
	perfAnalyser.backgroundTextTextureMemory:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.backgroundTextTextureMemory.x = screenOriginX
	perfAnalyser.backgroundTextTextureMemory.y = perfAnalyser.backgroundGraphTextureMemory.y + perfAnalyser.backgroundGraphTextureMemory.contentHeight
	perfAnalyser.backgroundTextTextureMemory:setFillColor(0,0,0)
	perfAnalyser.backgroundTextTextureMemory.alpha = 0.9
	
	perfAnalyser.textTextureMemory = display.newText("0",(contentWidth - screenOriginX*2) * 0.25, perfAnalyser.backgroundTextTextureMemory.y, "Arial", 14)
	perfAnalyser.textTextureMemory:setTextColor(255,255,255)
	perfAnalyser.textTextureMemory:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.textTextureMemory.x = (contentWidth - screenOriginX*2) * 0.25
	perfAnalyser.textTextureMemory.y =  perfAnalyser.backgroundTextTextureMemory.y
	perfAnalyser.group:insert(perfAnalyser.textTextureMemory)
	
	----------------------- MEMORY LUA -----------------------------------------------------------------------------------------
	---- Back Texture and Graph
	perfAnalyser.backgroundGraphLuaMemory = display.newRect(perfAnalyser.backgroundGraphTextureMemory.x +perfAnalyser.backgroundGraphTextureMemory.contentWidth
															,perfAnalyser.backgroundTextFPS.y + perfAnalyser.backgroundTextFPS.contentHeight,
					contentWidth/2 - screenOriginX *2, (contentHeight -screenOriginY *2)/15)
	perfAnalyser.group:insert(perfAnalyser.backgroundGraphLuaMemory)
	perfAnalyser.backgroundGraphLuaMemory:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.backgroundGraphLuaMemory.x = perfAnalyser.backgroundGraphTextureMemory.x +perfAnalyser.backgroundGraphTextureMemory.contentWidth
	perfAnalyser.backgroundGraphLuaMemory.y = perfAnalyser.backgroundTextFPS.y + perfAnalyser.backgroundTextFPS.contentHeight
	perfAnalyser.backgroundGraphLuaMemory:setFillColor(g)
	perfAnalyser.backgroundGraphLuaMemory.alpha = 0.9
	
	local paramsLuaMemoryGraph = {}
	paramsLuaMemoryGraph.minValue = 0
	paramsLuaMemoryGraph.maxValue = 50

	local surfaceRectLuaMemoryInstance = {object = perfAnalyser.backgroundGraphLuaMemory, displayGroupParent = perfAnalyser.group}
	paramsLuaMemoryGraph.surfaceObject = surfaceRectLuaMemoryInstance
	paramsLuaMemoryGraph.maxValuesInHistoric = perfAnalyser.backgroundGraphLuaMemory.contentWidth/3
	paramsLuaMemoryGraph.refreshInterval = 10
	paramsLuaMemoryGraph.lineColor = {0,0,255}
	perfAnalyser.graphLuaMemory = graphInstance.Graph.create(paramsLuaMemoryGraph)
	
	
	---- Back Lua Memory TEXTS
	perfAnalyser.backgroundTextLuaMemory = display.newRect( perfAnalyser.backgroundGraphTextureMemory.x +perfAnalyser.backgroundGraphTextureMemory.contentWidth
												,perfAnalyser.backgroundGraphLuaMemory.y + perfAnalyser.backgroundGraphLuaMemory.contentHeight,
												contentWidth/2 - screenOriginX *2, (contentHeight -screenOriginY *2)/30)
	perfAnalyser.group:insert(perfAnalyser.backgroundTextLuaMemory)
	perfAnalyser.backgroundTextLuaMemory:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.backgroundTextLuaMemory.x = perfAnalyser.backgroundGraphTextureMemory.x +perfAnalyser.backgroundGraphTextureMemory.contentWidth
	perfAnalyser.backgroundTextLuaMemory.y = perfAnalyser.backgroundGraphLuaMemory.y + perfAnalyser.backgroundGraphLuaMemory.contentHeight
	perfAnalyser.backgroundTextLuaMemory:setFillColor(0,0,0)
	perfAnalyser.backgroundTextLuaMemory.alpha = 0.9
	
	perfAnalyser.textLuaMemory = display.newText("0",(contentWidth - screenOriginX*2) *0.8, perfAnalyser.backgroundTextLuaMemory.y, "Arial", 14)
	perfAnalyser.textLuaMemory:setTextColor(255,255,255)
	perfAnalyser.textLuaMemory:setReferencePoint(display.TopLeftReferencePoint)
	perfAnalyser.textLuaMemory.x = (contentWidth - screenOriginX *2) *0.75
	perfAnalyser.textLuaMemory.y =  perfAnalyser.backgroundTextLuaMemory.y
	perfAnalyser.group:insert(perfAnalyser.textLuaMemory)
	
	
	perfAnalyser.lastFps = {};
	perfAnalyser.lastFpsCounter = 1;
	
	perfAnalyser.updateValues = function(event)
	
		local curTime = system.getTimer();
		local dt = curTime - prevTime;
		prevTime = curTime;

		-- Treat FPS 
		local fps = mFloor(1000/dt);
		
		perfAnalyser.lastFps[perfAnalyser.lastFpsCounter] = fps;

		perfAnalyser.lastFpsCounter = perfAnalyser.lastFpsCounter + 1;
		if(perfAnalyser.lastFpsCounter > maxSavedFps) then perfAnalyser.lastFpsCounter = 1; end
		
		local maxLastFps = maxElement(perfAnalyser.lastFps)
		local minLastFps = minElement(perfAnalyser.lastFps)
		local averageFPS = averageElement(perfAnalyser.lastFps)
		
		
		--Save current average FPS
		if(fps > maxSavedFps ) then fps = maxSavedFps end
		perfAnalyser.graphFPS:insertValue(maxSavedFps -fps)
		perfAnalyser.textFPS.text = "FPS: "..fps.." -- Average: "..averageFPS.." -- Min: "..minLastFps.." -- Max: ".. maxLastFps
		
		-- TreatTexture Memory
		local textureMemoryNumber = system.getInfo("textureMemoryUsed")/1000000
		local textureMemorySTR = string.format('%.3f',textureMemoryNumber)
		perfAnalyser.graphTextureMemory:insertValue(textureMemoryNumber)
		perfAnalyser.textTextureMemory.text = "Text Mem: "..textureMemorySTR.." Mb";
		
		local luaMemoryNumber = collectgarbage('count')/1000
		local luaMemorySTR = string.format('%.3f',luaMemoryNumber)
		
		--Save current average Memory
		perfAnalyser.graphLuaMemory:insertValue(luaMemoryNumber)
		perfAnalyser.textLuaMemory.text = "Lua Mem: "..luaMemorySTR.." Mb";
		
	end
	
	
	return perfAnalyser
	
end

function PerfAnalyser:start()

	self:stop()
	
	self.graphFPS:start()
	self.graphTextureMemory:start()
	self.graphLuaMemory:start()
	Runtime:addEventListener("enterFrame",self.updateValues)
	
end

function PerfAnalyser:stop()

	Runtime:removeEventListener("enterFrame",self.updateValues)
	self.graphFPS:stop()
	self.graphTextureMemory:stop()
	self.graphLuaMemory:stop()
end

