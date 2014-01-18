--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description RemoteControler is needed by Krea for Remote Control
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("remotecontroler", package.seeall)
local json = require("json")
local function split (s, delim)

  assert (type (delim) == "string" and string.len (delim) > 0,
          "bad delimiter")

  local start = 1
  local t = {}  -- results table

  -- find each instance of a string followed by the delimiter

  while true do
    local pos = string.find (s, delim, start, true) -- plain find

    if not pos then
      break
    end

    table.insert (t, string.sub (s, start, pos - 1))
    start = pos + string.len (delim)
  end -- while

  -- insert final one (after last delimiter)

  table.insert (t, string.sub (s, start))

  return t
 
end -- function split

RemoteControler = {}
RemoteControler.__index = RemoteControler

-- params = {}
-- params.ipAddress = 192.168.1.1
-- params.port = 8172

function RemoteControler.create(params)
	
	--Init attributes of instance
	local controler = {}           
	setmetatable(controler,RemoteControler)
	
	controler.ipAddress = params.ipAddress
	controler.port = params.port
	controler.isCleaning = false
	
	
	controler.onApplicationEvent = function(event)
		local errorLog = {}
		if event.type == "applicationStart" then
			controler:startClient(errorLog)
		elseif event.type == "applicationExit" then
			controler:clean()
		elseif event.type == "applicationSuspend" then
			controler:stopClient()
		elseif event.type == "applicationResume" then
			controler:startClient(errorLog)
		end
		
		for i = 1,#errorLog do 
			print(errorLog[i])
		end
	end
	
	Runtime:addEventListener("system",controler.onApplicationEvent)
	
	local errorsLog = {}
	controler:startClient(errorsLog)
	return controler,errorsLog
end


function RemoteControler:startClient(errorsLog)
	self.isCleaning = false
	self.socketLib = require("socket")
	self.clientSocket, err = assert(self.socketLib.tcp())
	if(err) then 
		print("Creation Socket error :"..err)
		table.insert(errorsLog,"Creation Socket error :"..err)
		return
	end
	
	local status, currentError = self.clientSocket:connect(self.ipAddress,self.port)
	self.clientSocket:settimeout(0)
	
	if(status == nil) then 
		print("Error : "..currentError)
		print(currentError)
		table.insert(errorsLog,"Connect Socket error :"..currentError)
		
		self:clean()
		
		return 
	else
		local numberStatus = tonumber(status)
		if( numberStatus == 1) then 
			status = "----- Krea: Connected to Remote Server!"
		else
			status = "----- Krea: Unable to connect to Remote Server!"
		end
		
		table.insert(errorsLog,"Status : "..status)
	end
	
	self.checkReceiveAndExecute = function()

		if(not self.isCleaning)then
			local data,err = self.clientSocket:receive('*l')
			
			if(data) then 
				
				self:checkAndDispatch(data)
			end
		end
	end
	
	self.idTimerCheckReceiveAndExecute = timer.performWithDelay(10,self.checkReceiveAndExecute,-1)
end

function RemoteControler:checkAndDispatch(message)

	if(message == "closed") then 
		print(message)
		self:clean()
	else
		
		if(message == "EXIT") then 
			self:clean()
			return
		end
		
		local event = json.decode(message)
		if(event) then 
			Runtime:dispatchEvent(event)
		else
			print("JSON Message decoding failed: "..message)
		end
	end
end

function RemoteControler:stopClient()

	if(not self.isCleaning) then 
		self.isCleaning = true
		local removeListener = function()
			if(self.idTimerCheckReceiveAndExecute) then 
				timer.cancel(self.idTimerCheckReceiveAndExecute)
			end
			
			self.clientSocket:send('EXIT\0')

			local resClose = self.clientSocket:close()
			local resShutDown = self.clientSocket:shutdown("both")
			print("Close? "..resClose.."---- ShutDown? "..resShutDown)
			self.socketLib = nil
			self.clientSocket = nil
		end
	end
	
	timer.performWithDelay(10,removeListener,1)
end


function RemoteControler:clean()
	if(not self.isCleaning) then 
		self.isCleaning = true
		if(self.clientSocket) then
			Runtime:removeEventListener("system",self.onApplicationEvent)
			self:stopClient()

		end	
	end
end