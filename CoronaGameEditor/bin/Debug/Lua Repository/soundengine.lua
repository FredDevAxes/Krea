--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Jonathan Jot
-- @copyright Native-Software 2012- All Rights Reserved.
-- @release 1.0 
-- @description Provides a SoundEngine managing all the audio resources and Channels for you.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("soundengine", package.seeall)


SoundEngine = {}
SoundEngine.__index = SoundEngine

--- 
-- @description Create a new SoundEngine Instance
-- @return Return a SoundEngine Instance
-- @param params Lua Table containing the params
-- @usage Usage:
-- <ul>
-- <li><code>local params = {}
-- <br>params.StreamList = {}
-- <br>params.SoundList = {}</li></br>
-- <li>local sound1 = {}
-- <br>sound1.Path = "soundFile.m4a"
-- <br>sound1.Volume = 14
-- <br>sound1.LoadOnInit = true
-- <br>sound1.Loops = -1
-- <br>sound1.Id = "sound1"</br></br></li>
-- <li>local stream1 = {}
-- <br>stream1.Path = "streamFile.m4a"
-- <br>stream1.Volume = 14
-- <br>stream1.LoadOnInit = true
-- <br>stream1.Loops = -1
-- <br>stream1.Id = "stream1"</li></br>
-- <li>params.StreamList[1] = stream1</li>
-- <li>params.SoundList[1] = sound1</li>
-- <li>local soundEngine = require("soundengine").SoundEngine.create(params)</li></code>

function SoundEngine.create(params)
   local soundEngineInstance = {}             -- our new object
   setmetatable(soundEngineInstance,SoundEngine)  
   
   -- Configure custom property
   --
   soundEngineInstance.StreamList = params.StreamList
   soundEngineInstance.SoundList = params.SoundList
   soundEngineInstance.isCurrentPlayingStream = false
   soundEngineInstance.isCurrentPlayingSound = false
   
   -- Preloading Selected Sound
   --
   	for i = 1, #soundEngineInstance.SoundList do 
		if(soundEngineInstance.SoundList[i].LoadOnInit == true) then
			soundEngineInstance:loadSound(soundEngineInstance.SoundList[i].Path)
			soundEngineInstance:playSound(soundEngineInstance.SoundList[i].Path)
		end 
	end
   
   -- Preloading SelectedStream
   --
    for i = 1, #soundEngineInstance.StreamList do 
		if(soundEngineInstance.StreamList[i].LoadOnInit == true) then
			soundEngineInstance:loadStream(soundEngineInstance.StreamList[i].Path)
			soundEngineInstance:playStream(soundEngineInstance.StreamList[i].Path)
		end 
	end
	
   return soundEngineInstance
end

--- 
-- @description Get Table of information for a Sound (Path, Volume, LoadOnInit, Loops, Instance)
-- @param SoundPath The sound file path
-- @usage Usage:
-- <ul>
-- <li><code>local SoundPath = "sound1.m4a"</li>
-- <li>soundEngine:getSoundInformation(SoundPath)</code></li>
-- </ul>
-- @return Audio Table Information or Nil if not exists
function SoundEngine:getSoundInformation(SoundPath)
	for i = 1, #self.SoundList do 
		if(self.SoundList[i].Path == SoundPath) then
			return self.SoundList[i]
		end 
	end
	return nil
end

--- 
-- @description Get Table of information for a Stream (Path, Volume, LoadOnInit, Loops, Instance)
-- @param StreamPath The stream file path
-- @usage Usage:
-- <ul>
-- <li><code>local StreamPath = "stream1.m4a"</li>
-- <li>soundEngine:getStreamInformation(StreamPath)</code></li>
-- </ul>
-- @return Audio Table Information or Nil if not exists
function SoundEngine:getStreamInformation(StreamPath)
	for i = 1, #self.StreamList do 
		if(self.StreamList[i].Path == StreamPath) then
			return self.StreamList[i]
		end 
	end
	return nil
end

--- 
-- @description Load a Sound into the memory
-- @param SoundPath The sound file path
-- @usage Usage:
-- <ul>
-- <li><code>local SoundPath = "sound1.m4a"</li>
-- <li>soundEngine:loadSound(SoundPath)</code></li>
-- </ul>
-- @return true if loaded, false otherwise
function SoundEngine:loadSound(SoundPath) 
	local SoundInstance = self:getSoundInformation(SoundPath)
	if (SoundInstance ~= nil) then
			SoundInstance.Instance = audio.loadSound(SoundInstance.Path)
			return true
	else
		return nil
	end 
end


--- 
-- @description Load a Stream into the memory
-- @param StreamPath The stream file path
-- @usage Usage:
-- <ul>
-- <li><code>local StreamPath = "stream1.m4a"</li>
-- <li>soundEngine:loadStream(StreamPath)</code></li>
-- </ul>
-- @return true if loaded, false otherwise
function SoundEngine:loadStream(StreamPath) 
	local StreamInstance = self:getStreamInformation(StreamPath)
	if (StreamInstance ~= nil) then
			StreamInstance.Instance = audio.loadStream(StreamInstance.Path)
			return true
	else
		return false
	end 
end


--- 
-- @description Play a Sound using its Volume Information and its Loops Information
-- @param SoundPath The sound file path
-- @usage Usage:
-- <ul>
-- <li><code>local SoundPath = "sound1.m4a"</li>
-- <li>soundEngine:playSound(SoundPath)</code></li>
-- </ul>
-- @return Channel if loaded, nil otherwise
function SoundEngine:playSound(SoundPath)
	
		local SoundInstance = self:getSoundInformation(SoundPath)
		if (SoundInstance ~= nil) then
			if(SoundInstance.Instance == nil) then 
				SoundInstance.Instance = audio.loadSound(SoundInstance.Path)
			end
		else 
			return nil
		end
			
		local availableChannel = audio.findFreeChannel()
		audio.setVolume( SoundInstance.Volume, { channel=availableChannel } ) 
		
		SoundInstance.Channels = audio.play(SoundInstance.Instance,{channel=availableChannel, loops=SoundInstance.Loops})
		availableChannel = nil
		self.isCurrentPlayingSound=true
		return SoundInstance.Channels
end

--- 
-- @description Play a Stream using its Volume Information and its Loops Information
-- @param StreamPath The stream file path
-- @usage Usage:
-- <ul>
-- <li><code>local StreamPath = "stream1.m4a"</li>
-- <li>soundEngine:playStream(StreamPath)</code></li>
-- </ul>
-- @return Channel if OK , nil otherwise
function SoundEngine:playStream(StreamPath)
		local StreamInstance = self:getStreamInformation(StreamPath)
		if (StreamInstance ~= nil) then
			if(StreamInstance.Instance == nil) then 
				StreamInstance.Instance = audio.loadStream(StreamInstance.Path)
			end
		else
			return nil
		end
		local availableChannel = audio.findFreeChannel()
		audio.setVolume( StreamInstance.Volume, { channel=availableChannel } ) 
	    StreamInstance.Channels = audio.play(StreamInstance.Instance,{channel=availableChannel, loops=StreamInstance.Loops})
		self.isCurrentPlayingSound=true
		availableChannel = nil
		return StreamInstance.Channels
end

--- 
-- @description Pause some channels
-- @param Channels The channels to be paused
-- @usage Usage:
-- <ul>
-- <li><code>soundEngine:pause(Channels)</code></li>
-- </ul>
function SoundEngine:pause(Channels)
	 return audio.pause(Channels)
end

--- 
-- @description Stop some channels
-- @param Channels The channels to be stoped
-- @usage Usage:
-- <ul>
-- <li><code>soundEngine:stop(Channels)</code></li>
-- </ul>
function SoundEngine:stop(Channels)
	return audio.stop(Channels)
end


--- 
-- @description Clean a Sound to free memomy
-- @param SoundPath to be cleaned
-- @usage Usage:
-- <ul>
-- <li><code>local SoundPath = "sound1.m4a"</li>
-- <li>soundEngine:cleanSound(SoundPath)</code></li>
-- </ul>
function SoundEngine:cleanSound(SoundPath)
		local SoundInstance = self:getSoundInformation(SoundPath)
		if (SoundInstance~=nil) then
			if(SoundInstance.Channels) then 
				self:stop(SoundInstance.Channels)
				SoundInstance.Channels = nil
			end
			audio.dispose(SoundInstance.Instance)
			SoundInstance.Instance = nil
			return true
		else
			return false
		end
end

--- 
-- @description Clean a Stream to free memomy
-- @param StreamPath to be cleaned
-- @usage Usage:
-- <ul>
-- <li><code>local StreamPath = "stream1.m4a"</li>
-- <li>soundEngine:cleanStream(StreamPath)</code></li>
-- </ul>
function SoundEngine:cleanStream(StreamPath)
		local StreamInstance = self:getStreamInformation(StreamPath)
		if (StreamInstance~=nil) then
			if(StreamInstance.Channels) then 
				self:stop(StreamInstance.Channels)
				StreamInstance.Channels = nil
			end
			
			audio.dispose(StreamInstance.Instance)
			StreamInstance.Instance = nil
			return true
		else
			return false
		end
end


