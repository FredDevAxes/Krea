--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description Graph is needed by Krea for Remote Control and FpsAnalyser
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("graph", package.seeall)

---- MATH HELPER FUNCTIONS ----------
local function distance ( u, v )

 -- just your regular distance formula
 
	local x = ( u.x - v.x ) 
	local y = ( u.y - v.y )  
		
	return math.sqrt( (x*x)+(y*y) );

end

local function polySimplify( linePoints , tolerance )

	local numPoints = #linePoints

	local nl = {}
 
	local  j, p;
 
	 nl[1] = linePoints[1];
	 
	 j = 2;
	 p = 1;
	 
	 for  i = 2, numPoints, 1  do
		
		  if ( distance(linePoints[i],linePoints[p]) < tolerance ) then
		   else
			 nl[j] = linePoints[i];
			 j = j+1;
			 p = i 
		  end
	 end
	  
	 if ( p  < numPoints -1 ) then
		nl[j] = linePoints[numPoints-1];
	 end
	 
	 return nl
 end

Graph = {}
Graph.__index = Graph

-- params = {}
-- params.minValue = 0
-- params.maxValue = 0
-- params.currentValue = 10
-- params.
-- params.port = 8172


function Graph.create(params)
	
	--Init attributes of instance
	local graph = {}           
	setmetatable(graph,Graph)
	
	graph.minValue = params.minValue
	graph.maxValue = params.maxValue
	graph.currentValue = params.currentValue
	graph.surfaceObject = params.surfaceObject
	graph.maxValuesInHistoric = params.maxValuesInHistoric
	graph.refreshInterval = params.refreshInterval
	graph.lineColor = params.lineColor
	
	graph.tabValues = {}
	if(graph.currentValue) then 
		table.insert(graph.tabValues,graph.currentValue)
	end

	graph.onRefresh = function()

		if(graph.displayLine) then 
			if(graph.displayLine.removeSelf) then 
				graph.displayLine:removeSelf()
			end
		end
		
		local tabValues = graph.tabValues
		if( tabValues and graph.surfaceObject.object.removeSelf) then 
			
			if(#tabValues >1) then 
				graph.surfaceObject.object:setReferencePoint(display.TopLeftReferencePoint)
				local objectWidth = graph.surfaceObject.object.contentWidth
				local objectHeight = graph.surfaceObject.object.contentHeight
				local objectX = graph.surfaceObject.object.x
				local objectY = graph.surfaceObject.object.y
				local minMaxValueInterval = graph.maxValue - graph.minValue
				
				
				local xIntervalBetweenEachPoints = objectWidth / graph.maxValuesInHistoric
				
				local linePoints = {}
				
				for i =1,#tabValues do 
					local currentValue = tabValues[i]		
					
					local destY =  objectY  +  objectHeight - (((currentValue - graph.minValue)/minMaxValueInterval)*objectHeight)
					local destX = objectX + (i-1)* xIntervalBetweenEachPoints
					
					table.insert(linePoints,{x=destX,y = destY})
					
				end
				
				local finalLine = polySimplify( linePoints , 3)
				if(#finalLine >1) then
					for i = 2,#finalLine do  
					
						local destX = finalLine[i].x
						local destY = finalLine[i].y
						
						if(i == 2) then 
							local lastX = finalLine[i-1].x
							local lastY = finalLine[i-1].y
							
							graph.displayLine = display.newLine(lastX,lastY,
												destX,destY)
							graph.displayLine.width = 2
							graph.displayLine:setColor(graph.lineColor[1],graph.lineColor[2],graph.lineColor[3])
							graph.surfaceObject.displayGroupParent:insert(graph.displayLine)
						else
							graph.displayLine:append(destX, destY)
						end
						
						
					end
				
				end
				
				graph.surfaceObject.object:setReferencePoint(display.CenterReferencePoint)
			end
		end
	end
	
	return graph
end

function Graph:insertValue(value)
	local numberValue = tonumber(value)
	if(numberValue) then 
		if(numberValue > self.maxValue) then numberValue = self.maxValue end
		if(numberValue < self.minValue) then numberValue = self.minValue end
		self.currentValue = numberValue
		table.insert(self.tabValues,numberValue)
				
		if(#self.tabValues > self.maxValuesInHistoric) then 
			table.remove(self.tabValues,1)
		end
		return true
	end
	
	return false
end

function Graph:start()
	self:stop()
	
	self.idTimerRefresh = timer.performWithDelay(self.refreshInterval,self.onRefresh,-1)
end

function Graph:stop()
	if(self.idTimerRefresh) then 
		timer.cancel(self.idTimerRefresh)
		self.idTimerRefresh = nil
	end
end

function Graph:clean()
	self:stop()
	
	if(self.displayLine) then 
		if(self.displayLine.removeSelf) then 
			self.displayLine:removeSelf()
		end
		self.displayLine = nil
	end
	self.minValue = nil
	self.maxValue = nil
	self.currentValue = nil
	self.surfaceObject = nil
	self.maxValuesInHistoric = nil
	self.refreshInterval = nil
	self.lineColor = nil
	
	self.tabValues = nil
end