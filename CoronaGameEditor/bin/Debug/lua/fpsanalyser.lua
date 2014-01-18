--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description FPSAnalyser is a module providing a banner showing the CPU and the memory usage. Very usefull to analyse the app.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>
module("fpsanalyser", package.seeall)

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

local function createLayout(self)

	local group = display.newGroup();
	
	
	self.height = (contentHeight - screenOriginY) * 0.1
	self.lastFPSXPoint = screenOriginX+1
	self.tabFPSAverageValues = {}
	self.maxCountValues = math.floor((contentWidth- screenOriginX*2) /2 /3 )
	self.tabMemoryValues = {}
	
	local backgroundGraphs = display.newRect( screenOriginX,screenOriginY,
					contentWidth- screenOriginX *2, self.height );
	group:insert(backgroundGraphs);
	backgroundGraphs:setReferencePoint(display.TopLeftReferencePoint)
	backgroundGraphs.x = screenOriginX
	backgroundGraphs.y = screenOriginY
	
	local g = graphics.newGradient({0,255,0},{255,0,0},"up")
	backgroundGraphs:setFillColor(g)
	backgroundGraphs.alpha = 0.9
	
	local backgroundTexts = display.newRect(screenOriginX ,screenOriginY +  self.height,
					contentWidth - screenOriginX*2, self.height);
	group:insert(backgroundTexts);
	backgroundTexts:setReferencePoint(display.TopLeftReferencePoint)
	backgroundTexts.x = screenOriginX
	backgroundTexts.y = screenOriginY +  self.height
	backgroundTexts:setFillColor(0,0,0)
	backgroundTexts.alpha = 0.4

	-- Create FPS components
	self.graphFPS = display.newLine(screenOriginX,self.height,screenOriginX +1,self.height)
	self.graphFPS:setColor(255,255,0)
	group:insert(self.graphFPS)
	
	self.framerate = display.newText("0",contentWidth * 0.25, self.height, "Arial", 14);
	self.framerate:setTextColor(255,255,255);
	self.framerate:setReferencePoint(display.TopLeftReferencePoint)
	self.framerate.x = contentWidth * 0.25 
	self.framerate.y =  self.height +10+screenOriginY
	group:insert(self.framerate);
		
	-- Create Memory components
	self.graphMemory = display.newLine(halfContentWidth,self.height /2,halfContentWidth+1,self.height)
	self.graphFPS:setColor(255,255,0)
	group:insert(self.graphFPS)
	
	self.memory = display.newText("0/10",contentWidth * 0.75,self.height, "Arial", 14);
	self.memory:setTextColor(255,255,255);
	self.memory:setReferencePoint(display.TopLeftReferencePoint)
	self.memory.x = contentWidth * 0.75
	self.memory.y = self.height +screenOriginY
	group:insert(self.memory);

	return group;
end
 
 local function updateFPSGraphLine(self)
	
	if(self) then 
	
		if(self.graphFPS.removeSelf) then 
			self.graphFPS:removeSelf()
		end
		
		if(self.tabFPSAverageValues) then 

			if(#self.tabFPSAverageValues >0) then 
				
				for i =1,#self.tabFPSAverageValues do 
					local destY =  self.height * self.tabFPSAverageValues[i]/30
					if(i ==1) then 
						self.graphFPS = display.newLine(screenOriginX,screenOriginY +destY,
												screenOriginX+i*3,screenOriginY +destY)
						self.graphFPS:setColor(255,255,0)
						self.group:insert(self.graphFPS)
					end
					self.graphFPS:append(screenOriginX+i*3,screenOriginY +destY)
				end
				
			end
		end
	end
 end
 
 local function updateMemoryGraphLine(self)
	
	
	
	if(self) then 
	
		if(self.graphMemory.removeSelf) then 
			self.graphMemory:removeSelf()
		end
		
		if(self.tabMemoryValues) then 

			if(#self.tabMemoryValues >0) then 
				
				for i =1,#self.tabMemoryValues do 
					local destY =  self.tabMemoryValues[i]
					if(i ==1) then 
						self.graphMemory = display.newLine(halfContentWidth,self.height -destY
										,halfContentWidth +i*3,self.height -destY + screenOriginY)
						self.graphMemory:setColor(0,0,255)
						self.group:insert(self.graphMemory)
					end
					self.graphMemory:append(halfContentWidth +i*3,self.height -destY + screenOriginY)
				end
				
			end
		end
	end
	
	
	
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
			table.insert(self.tabFPSAverageValues,fps)
			
			if(#self.tabFPSAverageValues > self.maxCountValues) then 
				table.remove(self.tabFPSAverageValues,1)
			end
			
			updateFPSGraphLine(self)
			self.framerate.text = "FPS: "..fps.." -- Average: "..averageFPS.."\n".."Min: "..minLastFps.." -- Max: ".. maxLastFps
			
			-- TreatMemory
			local currentMemory = system.getInfo("textureMemoryUsed")/1000000
			--Save current average Memory
			table.insert(self.tabMemoryValues,currentMemory)
			
			if(#self.tabMemoryValues > self.maxCountValues) then 
				table.remove(self.tabMemoryValues,1)
			end
			
			updateMemoryGraphLine(self)
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