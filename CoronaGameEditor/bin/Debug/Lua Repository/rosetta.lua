--- This module is provided  as it by Graham Ranson of MonkeyDead Studios.
-- <br>Class is MIT Licensed</br>
-- <br>http://www.grahamranson.co.uk</br>
-- <br>http://www.monkeydeadstudios.com</br>
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>
-- @copyright 2011 MonkeyDead Studios - All Rights Reserved.
-- @author Graham Ranson
-- @release 1.0 
module("rosetta", package.seeall)

local json = require("json")

local loadFileContents = function(file)
	local path = system.pathForFile( file ,system.ResourceDirectory )
	
	if path then
		local file = io.open( path, "r" )
	
		if file then -- nil if no file found
	   
			local contents = file:read( "*a" )
	   
		   	io.close( file )
	
			return contents
		else
			return nil
		end
	else
		return nil
	end
end

local decodeJsonData = function(data)
	if data then
		return json.decode(data)
	end
end

local loadSettings = function()

	local contents = loadFileContents("lang.set")

	
	local settings = nil
	if(contents) then 
		settings =decodeJsonData(contents)
		
	end
	return settings
end

local loadLanguage = function(name)
	local contents = loadFileContents(name .. ".set")
	local language = nil
	local languageFinal = nil
	if(contents) then 
		language = decodeJsonData(contents)

		if(language) then 
			languageFinal = {}
			for k,v in pairs(language) do 
				local finalK = string.gsub(k,"!NL!","\n")
				local finalV = string.gsub(v,"!NL!","\n")

				--print(k,v)
				--print(finalK,finalV)
				--print("----")
				languageFinal[finalK] = finalV
			end
		end
	end
	
	return languageFinal
end

function new()
	
	local self = {}
	
	self.languages = {}
	self.currentLanguage = nil
	self.supportedLanguages = {}
	
	function self:initiate()

		self.settings = loadSettings()
		
		-- loop through all specified languages loading up their files
		for i=1, #self.settings.languages, 1 do
			local name = self.settings.languages[i]
			local language = loadLanguage(name)
			
			-- only add it as a supported language if the language file exists
			if language then
				self.languages[name] = language
				
				self.supportedLanguages[#self.supportedLanguages] = name
			end
		end
		
		-- try to set the default language
		if self.settings.default then
			if self.languages[self.settings.default] then
				self:setCurrentLanguage(self.settings.default)
			end
		else
			print("Rosetta: No default language set in language.settings")
		end

	end
	
	-- Returns a list of all the supported languages
	function self:getSupportedLanguageNames()
		return self.supportedLanguages
	end
	
	-- Returns the language table the specified language
	function self:getLanguage(name)
		return self.languages[name]
	end
	
	-- Sets the current language to the one specified
	function self:setCurrentLanguage(name)

		local language = self:getLanguage(name)
		
		if language then
			self.currentLanguage = language
			self.currentLanguageName = name
		else
			print("Rosetta: Language not found: " .. name or "nil")
		end
		
	end
	
	-- Gets the current language
	function self:getCurrentLanguage()
		return self.currentLanguage
	end
	
	-- Gets the name of the current language
	function self:getCurrentLanguageName()
		return self.currentLanguageName
	end
	
	-- Gets a string from the current language or the one specified as the second argument
	function self:getString(stringName, languageName)
		
		local language = nil
		
		if languageName then
			language = self:getLanguage(languageName)
			
			if not language then -- if a language name has been passed in but language is null then the language isn't supported
				print("Rosetta: Language not supported: " .. languageName)
				
				return nil
			end
		else
			language = self.currentLanguage
		end
		
		if language then
			return language[stringName] or "NO STRING"
		else
			print("Rosetta: Current language not set via rosetta:setCurrentLanguage(name)")
		end
	end
	
	return self
	
end