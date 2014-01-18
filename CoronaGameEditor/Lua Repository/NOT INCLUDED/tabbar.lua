module(..., package.seeall)
--==============================================================================================
-- Corona Game Editor - Frederic Raimondi & Jonathan Jot - (c) Native-Software 2012- All Rights Reserved.
--==============================================================================================
-- Class TabBar
-- Version 1.0 
-- TabBar is a custom widget that can be include into a corona project.
--==============================================================================================
-- Licence: This module is provided by Native-Software for use into a Corona Game Editor project. 
-- Changing the content of this file is at your own risk. As the software Corona Game Editor uses
-- this file to auto-generate code, it can create errors during generation if you change his content.


local tabBar = nil
local isScreenShown = nil
local totalScreenWidth = display.contentWidth -  display.screenOriginX
local tourellesInstance = nil
local soldiersInstance = nil
local classeMereForcesInstance = nil

local isDroitier = nil
----- Class TabBar ----------
TabBar = {}
TabBar.__index = TabBar

function TabBar.create()
   local tabBar = {}             -- our new object
   setmetatable(tabBar,TabBar)  -- make Account handle lookup
   
   --Create Attributes
   tabBar.displayGroup = display.newGroup()
   tabBar.listButtons = display.newGroup()
   
   
   
   tabBar.idTransitionX = nil
   tabBar.isOpened = false
   tabBar.isMoving = false
   tabBar.tabScreens = {"screen1","screen2","screen3"}
   tabBar.currentScreen = nil
   tabBar.countBt = 0
   
   
   tabBar.background = display.newImageRect("Images/tabbar.png",49,display.contentHeight)
   tabBar.displayGroup:insert(tabBar.background)
   
   
   --Creer un buttuon pour ouvrir et fermer le tab
   local paramBtTabOpener = {
				name = "Open",
				pathImage = "Images/opentab.png",
				tabBarParent = tabBar,
				behaviour = "tabOpener"
	}
	tabBar.btOpen = BarButton.create(paramBtTabOpener)

   if(isDroitier == true ) then 
		tabBar.displayGroup:translate(totalScreenWidth - tabBar.background.contentWidth *0.5 ,display.contentHeight *0.5)
   else
		tabBar.displayGroup:translate(display.screenOriginX+ tabBar.background.contentWidth *0.5,display.contentHeight *0.5)
   end
   
   tabBar.displayGroup:toFront()
   
   tabBar.displayGroup:insert(tabBar.listButtons)
   
   tabBar:loadScreen(tabBar.tabScreens[1],nil)
   tabBar.displayGroup:insert(tabBar.currentScreen)
   
   local function onTouchTabBar(event)
		return true
   end
   
   return tabBar
end

function TabBar:open()
	if(self.isOpened == false) then 
		if(self.idTransitionX) then 
			transition.cancel(self.idTransitionX)
		end
		
		local onCompleteTransition = function()
			self.isOpened = true
			self.isMoving = false
		end
		
		self.isMoving = true
		
		if(isDroitier == true) then
			transition.to(self.displayGroup,{time=800, x = totalScreenWidth - self.currentScreen.contentWidth,onComplete = onCompleteTransition})
		else
			transition.to(self.displayGroup,{time=800, x = display.screenOriginX + self.currentScreen.contentWidth,onComplete = onCompleteTransition})
		end	
		
	end
end

function TabBar:close()
	if(self.isOpened == true) then 
		if(self.idTransitionX) then 
			transition.cancel(self.idTransitionX)
		end
		
		local onCompleteTransition = function()
			self.isOpened = false
			self.isMoving = false
		end
		
		self.isMoving = true
		
		if(isDroitier == true) then
			transition.to(self.displayGroup,{time=800, x = totalScreenWidth - self.background.contentWidth *0.5 ,
												onComplete = onCompleteTransition})
		else
			transition.to(self.displayGroup,{time=800, x = self.background.contentWidth *0.5 ,
												onComplete = onCompleteTransition})
		end
	end
end

function TabBar:loadScreen(newScreen,screenRequested)
	if self.currentScreen then
		self.currentScreen:cleanUp()
		self.currentScreen = nil
	end
	self.currentScreen = require(newScreen).new(screenRequested)
	self.displayGroup:insert(self.currentScreen)
	
	if(isDroitier == true) then 
		self.currentScreen:translate(self.currentScreen:getWidth() - self.background.contentWidth -2,self.background.y)
	else
		self.currentScreen:translate(-self.currentScreen:getWidth() +  self.background.contentWidth +2 ,self.background.y)
	end
	

end


--------CLASS BUTTON BAR -------------
BarButton = {}
BarButton.__index = BarButton

function BarButton.create(params)
   local barBt = {}             -- our new object
   setmetatable(barBt,BarButton)  -- make Account handle lookup

   -----Attributes
   barBt.parent = params.tabBarParent
	barBt.myName = params.name

   --Creer l'objet de rendu
	barBt.obj = display.newImageRect(params.pathImage,30,39)
	barBt.parent.listButtons:insert(barBt.obj)
	barBt.obj:toFront()
   
   
   --Creer un event touch sur l'objet
   ---------------------------
   if(params.behaviour == "screenChooser") then 
		barBt.id = barBt.parent.countBt
		barBt.parent.countBt = barBt.parent.countBt +1
		
		barBt.obj.x = barBt.parent.background.x 
		barBt.obj.y = -barBt.parent.background.contentHeight /2 + barBt.obj.contentHeight *(barBt.id+1) *1.5

	   barBt.onTouch = function(event)	
			local t = event.target
			local phase = event.phase
			if phase == "ended" then 
				if(barBt.parent.isMoving==false) then
					
					if(barBt.id ==0) then barBt.parent:loadScreen("screen1",nil)
					elseif(barBt.id ==1) then barBt.parent:loadScreen("screen2",nil)
					end
					
					if(barBt.parent.isOpened == false) then 
						barBt.parent:open()
					end
				end
			end
			
			return true
		end	
	---------------------------
	elseif(params.behaviour == "tabOpener") then 
		barBt.id = 99
		barBt.obj.x = barBt.parent.background.x 
		barBt.obj.y = barBt.parent.background.contentHeight /2 - barBt.obj.contentHeight /2
		barBt.parent.listButtons:insert(barBt.obj)
		barBt.parent.listButtons:insert(barBt.obj)
		barBt.onTouch = function(event)	
		
				local t = event.target
				local phase = event.phase
				if phase == "ended" then 
					if(barBt.parent.isMoving==false) then
						
						if(barBt.parent.isOpened == false) then 
							barBt.parent:open()
						else
							barBt.parent:close()
						end
					end
				end
				return true
			end	
	---------------------------
	elseif(params.behaviour == "selectArme1") then 
		barBt.id = barBt.parent.countBt
		barBt.parent.countBt = barBt.parent.countBt +1
		
		barBt.obj.x = barBt.parent.background.x 
		barBt.obj.y = -barBt.parent.background.contentHeight /2 + barBt.obj.contentHeight *(barBt.id+1) *1.5
		
		barBt.onTouch = function(event)
			if(event.phase == "ended") then 
				tourellesInstance.createTourelle(display.contentWidth /2,display.contentHeight /2
											,barBt.parent,classeMereForcesInstance)
			end
			return true
		end
	---------------------------
	elseif(params.behaviour == "selectArme2") then 
		barBt.id = barBt.parent.countBt
		barBt.parent.countBt = barBt.parent.countBt +1
		
		barBt.obj.x = barBt.parent.background.x 
		barBt.obj.y = -barBt.parent.background.contentHeight /2 + barBt.obj.contentHeight *(barBt.id+1) *1.5
		
		barBt.onTouch = function(event)
			if(event.phase == "ended") then 
				soldiersInstance.createSoldier(display.contentWidth /2,display.contentHeight /2,
												barBt.parent,classeMereForcesInstance)
			end
			return true
		end	
	end
	---------------------------
	--Create a label 
	barBt.label = display.newText(barBt.myName, 0, 0, native.systemFont, 10)
	barBt.label:setTextColor(255,255,255)
	barBt.label.x = barBt.obj.x 
	barBt.label.y = barBt.obj.y + barBt.label.contentHeight 
	barBt.parent.listButtons:insert(barBt.label)
	barBt.label:toFront()
	
	barBt:startTouchListener()
   
   return barBt
end

function BarButton:startTouchListener()
	self:stopTouchListener()
	self.obj:addEventListener("touch",self.onTouch)
end

function BarButton:stopTouchListener()
	 self.obj:removeEventListener("touch",self.onTouch)
end

--------------

function init(groupInstances,isDroitierPar)
	tourellesInstance = groupInstances.tourellesInstance
	soldiersInstance = groupInstances.soldiersInstance
	classeMereForcesInstance = groupInstances.classeMereForcesInstance
	isDroitier = isDroitierPar
	
	isScreenShown = false
	isTabInTransitionX = false
	--Create a group that contains the entire screen and tab bar
	mainView = display.newGroup()	

	--Create a group that contains the screens beneath the tab bar
	tabView = display.newGroup()	
	mainView:insert(tabView)

	tabBar = TabBar.create()
	local paramBtScreenChooser1 = { 
				name = "Screen 1",
				pathImage = "Images/tab1.png",
				tabBarParent = tabBar,
				behaviour = "screenChooser"
				
	}
	local paramBtScreenChooser2 = { 
				name = "Screen 2",
				pathImage = "Images/tab1.png",
				tabBarParent = tabBar,
				behaviour = "screenChooser"
				
	}
	
	
	
	local bt1 = BarButton.create(paramBtScreenChooser1)
	local bt2 = BarButton.create(paramBtScreenChooser2)
	
	--Creer un button de creation de l'arme 1 : TOURELLES
	local paramBtSelectArm1 = { 
				name = "Tourelle",
				pathImage = "Images/tab2.png",
				tabBarParent = tabBar,
				behaviour = "selectArme1"
	}
	
	local bt3 = BarButton.create(paramBtSelectArm1)
	
	--Creer un button de creation de l'arme 1 : TOURELLES
	local paramBtSelectArm2 = { 
				name = "Soldier",
				pathImage = "Images/tab2.png",
				tabBarParent = tabBar,
				behaviour = "selectArme2"
	}
	
	local bt4 = BarButton.create(paramBtSelectArm2)
	
	mainView:insert(tabBar.displayGroup)

	
	return tabBar
end


