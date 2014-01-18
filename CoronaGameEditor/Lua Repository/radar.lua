--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012- All Rights Reserved.
-- @release 1.0 
-- @see object
-- @description Provides a Radar Object allowing to see the position of referenced displayObject into the scene
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("radar", package.seeall)

--- 
-- Members of the Radar Instance
-- @class table
-- @name Fields
-- @field params The table params used to create the radar instance
-- @field tabObjects The multidimensional table containing the different categories of objects to display on the radar
-- @field surface The display object used as display radar surface
-- @field sceneWidth The scene width to display on the radar
-- @field sceneHeight The scene height to display on the radar
-- @field tabEchoColors The multidimensional table containing the different echo category colors to display on the radar
-- @field echoAlpha The alpha of the radar echos
-- @field refreshInterval The refresh interval of the radar
-- @field isEnabled Indicates whether the radar is enabled or not

Radar = {}
Radar.__index = Radar

--- 
-- @description Create a new Radar Instance
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.name = "radar"
-- <br>params.surface = displayObject
-- <br>params.sceneWidth = 230 
-- <br>params.sceneHeight = 480
-- <br>params.tabObjects = {{obj1,obj2},{obj3,obj4})
-- <br>params.tabEchoColors = {{R1,G1,B1},{R2,G2,B2}
-- <br>params.echoAlpha = 0.5
-- <br>params.refreshInterval = 100 </li>
-- <li>local radar = require("radar").Radar.create(params)</code> </li>
-- </ul>
-- @param params Lua Table containing the params
-- @return Radar Instance
function Radar.create(params)
	
	--Init attributes of instance
	local radar = {}           
	setmetatable(radar,Radar)
	
	radar.params = params
	if(params.tabObjects) then 
		radar.tabObjects = params.tabObjects
	else
		radar.tabObjects = {}
	end
	
	radar.surface = params.surface
	
	radar.sceneWidth = params.sceneWidth
	radar.sceneHeight = params.sceneHeight

	radar.tabEchoColors = params.tabEchoColors
	radar.echoAlpha = params.echoAlpha
	radar.refreshInterval = params.refreshInterval

	radar.isEnabled = false
	
	radar.refresh = function()
	
		if(radar.displayGroup) then 
			if(radar.displayGroup.removeSelf) then 
				radar.displayGroup:removeSelf()
			end
			radar.displayGroup = nil
		end
		
		radar.displayGroup = display.newGroup()
		radar.surface.displayGroupParent:insert(radar.displayGroup)
		radar.surface.object:setReferencePoint(display.TopLeftReferencePoint)
		
		radar.currentScaleX = radar.sceneWidth / radar.surface.object.contentWidth
		radar.currentScaleY = radar.sceneHeight /radar.surface.object.contentHeight
		
		local xMin = radar.surface.object.x 
		local yMin = radar.surface.object.y 
		local xMax = radar.surface.object.x + radar.surface.object.contentWidth
		local yMax = radar.surface.object.y + radar.surface.object.contentHeight
		
		for i = 1,#radar.tabObjects do 
			local tabIndexToRemove = {}
			for j = 1,# radar.tabObjects[i] do 
				local obj = radar.tabObjects[i][j]
				if(obj) then 
					if(obj.object.removeSelf) then 
						local radius = nil
						if(obj.object.contentWidth >= obj.object.contentHeight) then 
							radius = obj.object.contentWidth / radar.currentScaleX
						else
							radius = obj.object.contentHeight / radar.currentScaleY
						end
						
						local echoX = radar.surface.object.x + obj.object.x / radar.currentScaleX
						local echoY = radar.surface.object.y + obj.object.y / radar.currentScaleY
						
						if(echoX > xMin and echoX < xMax and echoY>yMin and echoY <yMax) then 
							local echo = display.newCircle(0,0,radius /2)
							
							echo:setFillColor(radar.tabEchoColors[i][1],radar.tabEchoColors[i][2],radar.tabEchoColors[i][3])
							echo.alpha = radar.echoAlpha
							radar.displayGroup:insert(echo)
							
							
							echo.x = echoX
							echo.y = echoY
						end

					else
						table.insert(tabIndexToRemove,j)
					end
				else
					table.insert(tabIndexToRemove,j)
				end
			end
			
			for j = 1,#tabIndexToRemove do 
				table.remove(radar.tabObjects[i],tabIndexToRemove[j])
			end
			
		end
	
	end
	
	return radar
end

--- 
-- @description Start to refresh objects position
-- @usage Usage:
-- <ul>
-- <li><code>radar:start()</code></li>
-- </ul></br>
function Radar:start()
	if(self.isEnabled ~= true) then 
		self.isEnabled = true
		self.idTimerResfreshing = timer.performWithDelay(self.refreshInterval,self.refresh,-1)
		
	end
	
end

--- 
-- @description Stop to refresh objects position
-- @usage Usage:
-- <ul>
-- <li><code>radar:stop()</code></li>
-- </ul></br>
function Radar:stop()
	self.isEnabled = false
	if(self.idTimerResfreshing) then 
		timer.cancel(self.idTimerResfreshing)
	end
end

--- 
-- @description Clean the Radar instance
-- @usage Usage:
-- <ul>
-- <li><code>radar:clean()</code></li>
-- <li><code>radar = nil</code></li>
-- </ul></br>
function Radar:clean()
	
	self.params = nil
	self.tabObjects = nil

	
	self.surface =nil
	
	self.sceneWidth = nil
	self.sceneHeight = nil

	self.tabEchoColors = nil
	self.echoAlpha =nil
	self.refreshInterval = nil

	self.refresh = nil
	
	 if(self.displayGroup) then 
		if(self.displayGroup.removeSelf) then 
			self.displayGroup:removeSelf()
		end
		self.displayGroup = nil
	 end	
	
end
