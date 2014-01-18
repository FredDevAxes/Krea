-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

-- include the Corona "storyboard" module
local storyboard = require "storyboard"

local function init()

--AUDIO
soundEngineInstance = nil
audioEngine = nil

--LANGUAGES
-- Load Rosetta
rosetta = require("rosetta").new()

-- Initiate Rosetta - this will load all languages and settings
-- rosetta:initiate()
 
--======================================================
-- Begin of auto-generated section: DO NOT MODIFY
--======================================================
----BODY:START
	
	
----BODY:END
--======================================================
-- End of auto-generated section 
--======================================================	
end

---------------------------------------------------------------------------------------
----------------------- Utilities Functions : DO NOT REMOVE ---------------------------
---------------------------------------------------------------------------------------
function getLastestFileTime(pathFileTime1,pathFileTime2) 
	--Check if first time file exists
	local timeFile1 = io.open(pathFileTime1)
	if(timeFile1 ==nil ) then 
		local result = "ERROR: "..string.format(pathFileTime1).." does not exists !"
		return nil,result
	end
	
	--Check if second time file exists
	local timeFile2 = io.open(pathFileTime2)
	if(timeFile2 ==nil ) then 
		timeFile1:close()
		local result = "ERROR: "..string.format(pathFileTime2).." does not exists !"
		return nil,result
	end
	
	--Read the two files and convert the results in numbers
	local time1 = timeFile1:read("*n")
	if(time1== nil) then 
		timeFile1:close()
		timeFile2:close()
		local result = "ERROR: the content of"..string.format(pathFileTime1).." is not a number!"
		return nil,result
	end
	
	local time2 = timeFile2:read("*n")
	if(time2== nil) then 
		timeFile1:close()
		timeFile2:close()
		local result = "ERROR: the content of"..string.format(pathFileTime2).." is not a number!"
		return nil,result
	end
	
	timeFile1:close()
	timeFile2:close()
	
	 --Compare the two numbers
	 if(time1 >= time2) then 
		local result = "Compare success !"
		return pathFileTime1,result
	 else
		local result = "Compare success !"
		return pathFileTime2,result
	 end
end

function copyFile( srcName, srcPath, dstName, dstPath, overwrite )
 
    local results = true                -- assume no errors
 
    -- Copy the source file to the destination file
    --
    local rfilePath = system.pathForFile( srcName, srcPath )
    local wfilePath = system.pathForFile( dstName, dstPath )
 
	
	local destFileExists = io.open(wfilePath)
	if(destFileExists ~=nil ) then 
		if(overwrite == false) then 
			print("File already exists, copy cancelled !")
			return
		end
		
		destFileExists:close()
	end
	
    local rfh = io.open( rfilePath, "rb" )              
    local wfh = io.open( wfilePath, "wb" )
        
    if  not wfh then
        print( "writeFileName open error!" )
        results = false                 -- error
    else
        -- Read the file from the Resource directory and write it to the destination directory
        local data = rfh:read( "*a" )
                
        if not data then
            print( "read error!" )
            results = false     -- error
        else
            if not wfh:write( data ) then
                print( "write error!" ) 
                results = false -- error
            end
        end
    end
        
        -- Clean up our file handles
        rfh:close()
        wfh:close()
 
        return results  
end

init()