module(..., package.seeall)

--==============================================================================================
-- Corona Game Editor - Frederic Raimondi & Jonathan Jot - (c) Native-Software 2012- All Rights Reserved.
--==============================================================================================
-- Module Ui
-- Version 1.0 
-- Ui provides custom widgets to include into a corona project like ProgressBar, Buttons...
--==============================================================================================
-- Licence: This module is provided by Native-Software for use into a Corona Game Editor project. 
-- Changing the content of this file is at your own risk. As the software Corona Game Editor uses
-- this file to auto-generate code, it can create errors during generation if you change his content.

--------------------------------------------------------------
------------ CLASS PROGRESS BAR -----------------------------
--------------------------------------------------------------

ProgressBar = {}
ProgressBar.__index = ProgressBar

---Construtor de la classe
--Model params Progress Bar
-- local params= {
			-- name = "Bouclier",
			-- x = -50,
			-- y = background.contentWidth *(0.5),
			-- width = 100,
			-- height = 10,
			-- defaultValue = 30,
			-- maxValue = 100,
			-- displayGroupParent = g,
			-- backColor = { R = 0,
						-- G = 0,
						-- B = 250},
						
			-- frontColor = { R = 110,
						-- G = 110,
						-- B = 0},

			-- textColor = { R = 110,
						-- G = 110,
						-- B = 0},
		
		-- }	
		
function ProgressBar.create(params)
	
	--Init attributes of instance
	local pBar = {}           
	setmetatable(pBar,ProgressBar)
	
	
	--Init values
	pBar.name = params.name
	pBar.displayGroupParent = params.displayGroupParent
	pBar.displayGroup = display.newGroup()
	pBar.displayGroupParent:insert(pBar.displayGroup)
	
	pBar.width = params.width
	pBar.height = params.height
	pBar.currentValue = params.defaultValue
	pBar.maxValue = params.maxValue

	pBar.objDisplayInterne = nil
	pBar.x = params.x
	pBar.y = params.y
	
	pBar.frontColor = params.frontColor
	pBar.backColor = params.backColor
	pBar.textColor = params.textColor 
	
	--Create rect externe
	pBar.objDisplayExterne = display.newRect(pBar.x,pBar.y, pBar.width,pBar.height)
	pBar.objDisplayExterne:setReferencePoint(display.CenterLeftReferencePoint)
	pBar.objDisplayExterne:setFillColor(pBar.backColor.R,pBar.backColor.G,pBar.backColor.B)
	pBar.displayGroup:insert(pBar.objDisplayExterne)
	
	--Create text with progress bar name
	pBar.objName = display.newText(pBar.name,0,0,native.systemFontBold, 10)
	pBar.objName.x = pBar.objDisplayExterne.x + pBar.objName.contentWidth *(0.5)
	pBar.objName.y = pBar.y - pBar.objName.contentHeight *(0.5)
	pBar.objName:setTextColor(pBar.textColor.R,pBar.textColor.G,pBar.textColor.B)
	pBar.displayGroup:insert(pBar.objName)
	
	--Create text with progress bar name
	pBar.objPercent = display.newText(pBar.currentValue.."%",0,0,native.systemFontBold, 10)
	pBar.objPercent.x = pBar.objDisplayExterne.x + pBar.objDisplayExterne.contentWidth  - pBar.objPercent.contentWidth *(0.5)
	pBar.objPercent.y = pBar.y - pBar.objPercent.contentHeight *(0.5)
	pBar.objPercent:setTextColor(pBar.textColor.R,pBar.textColor.G,pBar.textColor.B)
	pBar.displayGroup:insert(pBar.objPercent)
	
	--Create interior progress bar
	pBar:refresh()
	
	return pBar
end

function ProgressBar:refresh()

	--Calculer la largeur du rectangle
	local width = self.currentValue *(self.maxValue/self.width) * (self.maxValue/100)
	if(self.objDisplayInterne ~= nil) then 
		if(self.objDisplayInterne.removeSelf) then 
			self.displayGroup:remove(self.objDisplayInterne)
		end
		self.objDisplayInterne = nil
	end
	
	self.objPercent.text = width.."%"
	self.objDisplayInterne = display.newRect(self.x,self.y, width,self.height)
	self.objDisplayInterne:setFillColor(self.frontColor.R,self.frontColor.G,self.frontColor.B)
	self.displayGroup:insert(self.objDisplayInterne)
end

function ProgressBar:clean()
	if(self.displayGroup) then 
		self.displayGroup:removeSelf()
		self.displayGroup = nil
		
		self.width = nil
		self.height = nil
		self.currentValue = nil
		self.maxValue =nil

		self.objDisplayInterne = nil
		self.x =nil
		self.y = nil
		
		self.frontColor = nil
		self.backColor =nil
		self.textColor =nil
		
		self.objDisplayExterne = nil
		
		self.objName = nil

		self.objPercent = nil
	
	end
end

--------------------------------------------------------------
----------------  CLASSE BOUTTON ----------------------------
--------------------------------------------------------------
Button = {}
Button.__index = Button

--Model params Button
-- local params= {
			-- name = "btAttack",
			-- x = -50,
			-- y = background.contentWidth *(0.5),
			-- width = 100,
			-- height = 10,

			-- displayGroupParent = g,
			-- backColor = { R = 0,
						-- G = 0,
						-- B = 250},
						
			-- pathImageBack = "Images/bt.png"
			-- textColor = { R = 110,
						-- G = 110,
						-- B = 0},
		
		-- }	
		
function Button.create(params)
	--Init attributes of instance
	local bt = {}             -- our new object
	setmetatable(bt,Button) 
	
	--Init attributes
	bt.name = params.name
	bt.displayGroupParent = params.displayGroupParent
	
	--Check if pathImage initalized
	if(params.pathImageBack) then 
		--Create a button from a image
		bt.obj = display.newImageRect(params.pathImageBack,params.width,params.height)
		bt.obj.x = params.x
		bt.obj.y = params.y
		
	else
		--Create a rect with expected colors
		bt.obj = display.newRect(params.x,params.y,params.width,params.height)
		bt.obj:setFillColor(params.backColor.R,params.backColor.G,params.backColor.B)
	end
	bt.displayGroupParent:insert(bt.obj)
	
	if(bt.name) then 
		--Create a label 
		bt.labelObj = display.newText(bt.name,bt.obj.x,bt.obj.y + bt.obj.contentHeight *0.5,
									native.systemFont, 10)
		bt.labelObj:setTextColor(params.textColor.R,params.textColor.G,params.textColor.B)
		bt.displayGroupParent:insert(bt.labelObj)
	end
	
	return bt
end


--------------------------------------------------------------
----------------  CLASSE Tab Icon ----------------------------
--------------------------------------------------------------
TabIcon = {}
TabIcon.__index = TabIcon

--Model params TabIcon
-- local params= {
			-- name = "btAttack",
			-- x = -50,
			-- y = background.contentWidth *(0.5),
			-- width = 100,
			-- height = 10,

			-- displayGroupParent = g,
			-- backColor = { R = 0,
						-- G = 0,
						-- B = 250},
						
			-- pathImageBack = "Images/bt.png"
			
		
		-- }	
		
function TabIcon.create(params)
	--Init attributes of instance
	local tabIcon = {}             -- our new object
	setmetatable(tabIcon,TabIcon) 
	
	tabIcon.displayGroupParent = params.displayGroupParent
	
	--Create a display group
	tabIcon.displayGroup = display.newGroup()
	tabIcon.displayGroupParent:insert(tabIcon.displayGroup)
	
		--Init attributes
	tabIcon.name = params.name
	tabIcon.tabIcons = {}
	
	--Check if pathImage initalized
	if(params.pathImageBack) then 
		--Create a button from a image
		tabIcon.obj = display.newImageRect(params.pathImageBack,params.width,params.height)
		tabIcon.obj.x = params.x
		tabIcon.obj.y = params.y
		
		
	else
		--Create a rect with expected colors
		tabIcon.obj = display.newRect(params.x,params.y,params.width,params.height)
		tabIcon.obj:setFillColor(params.backColor.R,params.backColor.G,params.backColor.B)
	end
	
	tabIcon.displayGroup:insert(tabIcon.obj)
	tabIcon.displayGroup:toFront()
	tabIcon.obj.blendMode = "add"
	
	return tabIcon

end

--Idem Params bt 
function TabIcon:addIcon(params)

	--Calcul point x 
	local xDest = nil
	
	if(#self.tabIcons < 1 ) then 
		xDest  = self.obj.x - self.obj.contentWidth *(0.5) + 10
	else
		xDest = self.obj.x - self.obj.contentWidth *(0.5)+ (#self.tabIcons * 10)
	end
	
	params.x = xDest
	params.y = self.obj.y
	local icon =  Button.create(params)
	table.insert(self.tabIcons,icon)
	
end

function TabIcon:removeIcon(icon)

	for i = 1, #self.tabIcon do 
		if(icon == self.tabIcon[i]) then
			table.remove(self.tabIcons,i)
			
			self.displayGroup:remove(icon)
			icon = nil
			print("icon deleted")
		end
	end
end

	