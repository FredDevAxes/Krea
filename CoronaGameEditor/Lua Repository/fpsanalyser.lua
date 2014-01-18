--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description FPSAnalyser is a module providing a banner showing the CPU and the memory usage. Very usefull to analyse the app.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>
module("fpsanalyser", package.seeall)

local graphInstance = require("graph")

PerformanceOutput = {};
PerformanceOutput.mt = {};
PerformanceOutput.mt.__index = PerformanceOutput;

 
local prevTime = 0;
local maxSavedFps = 30;
 
local screenOriginX = display.screenOriginX
local screenOriginY = display.screenOriginY
local contentWidth = display.contentWidth
local contentHeight = display.contentHeight

local halfContentWidth = contentWidth * 0.5
local halfContentHeight = contentHeight * 0.5

local graphFPS = nil
local graphMemory = nil

local function createLayout(self)

	local group = display.newGroup();
	
	
	self.height = (contentHeight - screenOriginY) * 0.1
	
	local backgroundGraphFPS = display.newRect( screenOriginX,screenOriginY,
					contentWidth /2 - screenOriginX *2, self.height );
	group:insert(backgroundGraphFPS);
	backgroundGraphFPS:setReferencePoint(display.TopLeftReferencePoint)
	backgroundGraphFPS.x = screenOriginX
	backgroundGraphFPS.y = screenOriginY
	
	local backgroundGraphMemory = display.newRect( contentWidth /2 - screenOriginX *2,screenOriginY,
					contentWidth /2 - screenOriginX *2, self.height );
	group:insert(backgroundGraphMemory);
	backgroundGraphMemory:setReferencePoint(display.TopLeftReferencePoint)
	backgroundGraphMemory.x = contentWidth /2 - screenOriginX *2
	backgroundGraphMemory.y = screenOriginY
	
	local g = graphics.newGradient({0,255,0},{255,0,0},"up")
	
	backgroundGraphFPS:setFillColor(g)
	backgroundGraphFPS.alpha = 0.9
	
	backgroundGraphMemory:setFillColor(g)
	backgroundGraphMemory.alpha = 0.9
	
	local backgroundTexts = display.newRect(screenOriginX ,screenOriginY +  self.height,
					contentWidth - screenOriginX*2, self.height);
	group:insert(backgroundTexts);
	backgroundTexts:setReferencePoint(display.TopLeftReferencePoint)
	backgroundTexts.x = screenOriginX
	backgroundTexts.y = screenOriginY +  self.height
	backgroundTexts:setFillColor(0,0,0)
	backgroundTexts.alpha = 0.4

	-- Create FPS GRAPH
	local paramsFPSGraph = {}
	paramsFPSGraph.minValue = 0
	paramsFPSGraph.maxValue = 30

	local surfaceRectFPSInstance = {object = backgroundGraphFPS, displayGroupParent = group}
	paramsFPSGraph.surfaceObject = surfaceRectFPSInstance
	paramsFPSGraph.maxValuesInHistoric = backgroundGraphFPS.contentWidth/3
	paramsFPSGraph.refreshInterval = 10
	paramsFPSGraph.lineColor = {255,255,0}
	graphFPS = require("graph").Graph.create(paramsFPSGraph)
	
	-- Create Memory GRAPH
	local paramsMemoryGraph = {}
	paramsMemoryGraph.minValue = 0
	paramsMemoryGraph.maxValue = 30

	local surfaceRectMemoryInstance = {object = backgroundGraphMemory, displayGroupParent = group}
	paramsMemoryGraph.surfaceObject = surfaceRectMemoryInstance
	paramsMemoryGraph.maxValuesInHistoric = backgroundGraphMemory.contentWidth/3
	paramsMemoryGraph.refreshInterval = 10
	paramsMemoryGraph.lineColor = {255,255,0}
	graphMemory = require("graph").Graph.create(paramsMemoryGraph)
	
	self.framerate = display.newText("0",contentWidth * 0.25, self.height, "Arial", 14);
	self.framerate:setTextColor(255,255,255);
	self.framerate:setReferencePoint(display.TopLeftReferencePoint)
	self.framerate.x = contentWidth * 0.25 
	self.framerate.y =  self.height +10+screenOriginY
	group:insert(self.framerate);
		
	self.memory = display.newText("0/10",contentWidth * 0.75,self.height, "Arial", 14);
	self.memory:setTextColor(255,255,255);
	self.memory:setReferencePoint(display.TopLeftReferencePoint)
	self.memory.x = contentWidth * 0.75
	self.memory.y = self.height +screenOriginY
	group:insert(self.memory);

	return group;
end
 
 
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
        return math.floor(average);
end

 local function updateValues(self)
	local lastFps = {};
	local lastFpsCounter = 1;

	 return function(event)
			local curTime = system.getTimer();
			local dt = curTime - prevTime;
			prevTime = curTime;

			-- Treat FPS 
			local fps = math.floor(1000/dt);
			
			lastFps[lastFpsCounter] = fps;

			lastFpsCounter = lastFpsCounter + 1;
			if(lastFpsCounter > maxSavedFps) then lastFpsCounter = 1; end
			
			local maxLastFps = maxElement(lastFps)
			local minLastFps = minElement(lastFps)
			local averageFPS = averageElement(lastFps)
			
			
			--Save current average FPS
			if(fps > maxSavedFps ) then fps = maxSavedFps end
			-- graphFPS:insertValue(fps)
			
			self.framerate.text = "FPS: "..fps.." -- Average: "..averageFPS.."\n".."Min: "..minLastFps.." -- Max: ".. maxLastFps
			
			-- TreatMemory
			local currentMemory = system.getInfo("textureMemoryUsed")/1000000
			--Save current average Memory
			graphMemory:insertValue(tonumber(currentMemory))
			self.memory.text = "Mem: "..currentMemory.." mb";
        end
 end
 
local instance = nil;

--- 
-- @description Create a new FPSAnalyser Instance
-- @return Return a FPSAnalyser Instance
-- @usage Usage:
-- <ul>
-- <li> <code>local fpsanalyser = require("fpsanalyser").PerformanceOutput.new()</code></li>
-- </ul>
function PerformanceOutput.new()
        if(instance ~= nil) then return instance; end
        local self = {};
        setmetatable(self, PerformanceOutput.mt);
        
        self.group = createLayout(self);
		
		
        Runtime:addEventListener("enterFrame", updateValues(self));
		
		
        instance = self;
        return self;
end