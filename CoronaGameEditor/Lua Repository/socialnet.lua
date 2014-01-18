--- This module is provided  as it by Native-Software for use into a Krea project.
-- @author Frederic Raimondi, based on the Facebook Connect sample app provided by CoronaLabs,Inc.
-- @copyright Native-Software 2012 - All Rights Reserved.
-- @release 1.0 
-- @description Provide a module managing all the user need to post on the social networks such as facebook and tweeter.
-- <br>Changing the content of this file is at your own risk.
-- As the software Krea uses this file to auto-generate code, it can create errors during generation if you change his content.</br>

module("socialnet", package.seeall)

-- Load external library
local facebook = require("facebook")
local json = require("json")

-- Facebook Commands
local LOGOUT = 1
local SHOW_DIALOG = 2
local POST_MSG = 3
local POST_PHOTO = 4
local GET_USER_INFO = 5
local GET_PLATFORM_INFO = 6

-- This function is useful for debugging problems with using FB Connect's web api,
-- e.g. you passed bad parameters to the web api and get a response table back
local function printTable( t, label, level )
	if label then print( label ) end
	level = level or 1

	if t then
		for k,v in pairs( t ) do
			local prefix = ""
			for i=1,level do
				prefix = prefix .. "\t"
			end

			print( prefix .. "[" .. tostring(k) .. "] = " .. tostring(v) )
			if type( v ) == "table" then
				print( prefix .. "{" )
				printTable( v, nil, level + 1 )
				print( prefix .. "}" )
			end
		end
	end
end
----------------------------------------------------------------------------------------------------------------------
---------------- CLASS FacebookManager -----------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------
FacebookManager = {}
FacebookManager.__index = FacebookManager

function FacebookManager.create(params)
	local faceBManager = {}             -- our new object
	setmetatable(faceBManager,FacebookManager)  -- make Account handle lookup
	
	faceBManager.appID = params.appID
	if(not faceBManager.appID) then 
		-- Handle the response from showAlert dialog boxbox
		--
		local function onComplete( event )
			if event.index == 1 then
				system.openURL( "http://developers.facebook.com/get_started.php" )
			end
		end

		native.showAlert( "Error", "To develop for Facebook Connect, you need to get an API key and application secret. This is available from Facebook's website.",
			{ "Learn More", "Cancel" }, onComplete )
	end
	
	faceBManager.apiKey = params.apiKey
	
	faceBManager.delegate = nil
	faceBManager.statusMessageObject = params.statusMessageObject
	
	faceBManager.fbCommand = nil
	faceBManager.message = nil
	faceBManager.attachment = nil
	faceBManager.userInfo = nil
	
	faceBManager.listener = function(event)
	
		-- After a successful login event, send the FB command
		-- Note: If the app is already logged in, we will still get a "login" phase
		--
		if ( "session" == event.type ) then
			-- event.phase is one of: "login", "loginFailed", "loginCancelled", "logout"
			if(event.phase == "login")then 
				if(faceBManager.delegate and faceBManager.delegate.loginSuccess) then 
					faceBManager.delegate.loginSuccess()
				end
			elseif(event.phase == "loginFailed")then 
				if(faceBManager.delegate and faceBManager.delegate.loginFailed) then 
					faceBManager.delegate.loginFailed()
				end
			elseif(event.phase == "loginCancelled")then 
				if(faceBManager.delegate and faceBManager.delegate.loginCancelled) then 
					faceBManager.delegate.loginCancelled()
				end
			elseif(event.phase == "logout")then 
				if(faceBManager.delegate and faceBManager.delegate.logoutSucess) then 
					faceBManager.delegate.logoutSucess()
				end
			end
			
			
			
			print( "Session Status: " .. event.phase )
			
			if event.phase ~= "login" then
				-- Exit if login error
				return
			end
			
			-- The following displays a Facebook dialog box for posting to your Facebook Wall
			if faceBManager.fbCommand == SHOW_DIALOG then
				facebook.showDialog( {action="stream.publish"} )
			end
		
			-- Request the Platform information (FB information)
			if faceBManager.fbCommand == GET_PLATFORM_INFO then
				facebook.request( "platform" )		-- **tjn Displays info about Facebook platform
			end

			-- Request the current logged in user's info
			if faceBManager.fbCommand == GET_USER_INFO then
				facebook.request( "me" )
	--			facebook.request( "me/friends" )		-- Alternate request
			end

			-- This code posts a photo image to your Facebook Wall
			--
			if faceBManager.fbCommand == POST_PHOTO then
				
				if(faceBManager.attachment) then 
					facebook.request( "me/feed", "POST", faceBManager.attachment )		-- posting the photo
					faceBManager.attachment = nil
				end
			end
			
			-- This code posts a message to your Facebook Wall
			if faceBManager.fbCommand == POST_MSG then
				if(faceBManager.message) then 
					facebook.request( "me/feed", "POST", faceBManager.message )		-- posting the message
					faceBManager.message = nil
				end
			end
	-----------------------------------------------------------------------------------------

		elseif ( "request" == event.type ) then
			-- event.response is a JSON object from the FB server
			local response = event.response
			
			if ( not event.isError ) then
				response = json.decode( event.response )
				
				if faceBManager.fbCommand == GET_PLATFORM_INFO then
					if(faceBManager.statusMessageObject) then 
						faceBManager.statusMessageObject.text = response.name
					end
					faceBManager.platformInfo = response
					printTable( response, "Platform Info", 3 )
					
				elseif faceBManager.fbCommand == GET_USER_INFO then
					if(faceBManager.statusMessageObject) then 
						faceBManager.statusMessageObject.text = response.name
					end
					faceBManager.userInfo = response
					printTable( response, "User Info", 3 )
					print( "name", response.name )
					
				elseif faceBManager.fbCommand == POST_PHOTO then
					printTable( response, "photo", 3 )
					if(faceBManager.statusMessageObject) then 
						faceBManager.statusMessageObject.text = "Photo Posted"
					end
								
				elseif faceBManager.fbCommand == POST_MSG then
					printTable( response, "message", 3 )
					if(faceBManager.statusMessageObject) then 
						faceBManager.statusMessageObject.text = "Message Posted"
					end
					
				else
					-- Unknown command response
					print( "Unknown command response" )
					if(faceBManager.statusMessageObject) then 
						faceBManager.statusMessageObject.text = "Unknown ?"
					end
				end

			else
				-- Post Failed
				if(faceBManager.statusMessageObject) then 
					faceBManager.statusMessageObject.text = "Post Failded!"
				end
				printTable( event.response, "Post Failed Response", 3 )
			end
			
		elseif ( "dialog" == event.type ) then
			-- showDialog response
			--
			print( "dialog response:", event.response )
			if(faceBManager.statusMessageObject) then 
				faceBManager.statusMessageObject.text = event.response
			end
		end

	end
	
	
	
	return faceBManager
	
end

-- local attachment = {
	-- name = "Developing a Facebook Connect app using the Corona SDK!",
	-- link = "http://developer.anscamobile.com/forum",
	-- caption = "Link caption",
	-- description = "Corona SDK for developing iOS and Android apps with the same code base.",
	-- picture = "http://developer.anscamobile.com/demo/Corona90x90.png",
	-- actions = json.encode( { { name = "Learn More", link = "http://anscamobile.com" } } )
-- }
function FacebookManager:postPhoto(attachment)
	if(self.appID and attachment)then 
		self.attachment = attachment
		self.fbCommand = POST_PHOTO
		facebook.login( self.appID, self.listener,  {"publish_stream"}  )
	end
end

function FacebookManager:postMessage(message)
	if(self.appID and message)then 
		self.message = message
		self.fbCommand = POST_MSG
		facebook.login( self.appID, self.listener,  {"publish_stream"}  )
	end
end

function FacebookManager:getUserInfo()
	if(self.appID)then 
		self.userInfo = nil
		self.fbCommand = GET_USER_INFO
		facebook.login( self.appID, self.listener,  {"publish_stream"}  )
	end
end

function FacebookManager:getPlatformInfo()
	if(self.appID)then 
		self.platformInfo = nil
		self.fbCommand = GET_PLATFORM_INFO
		facebook.login( self.appID, self.listener,  {"publish_stream"}  )
	end
end

function FacebookManager:showDialog()
	if(self.appID)then 
		self.fbCommand = SHOW_DIALOG
		facebook.login( self.appID, self.listener,  {"publish_stream"}  )
	end
end

function FacebookManager:logOut()
	if(self.appID)then 
		self.fbCommand = LOGOUT
		facebook.logout()
	end
end

----------------------------------------------------------------------------------------------------------------------
---------------- CLASS TwitterManager -----------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------

local http = require("socket.http")
local ltn12 = require("ltn12")
local crypto = require("crypto")
local mime = require("mime")


TwitterManager = {}
TwitterManager.__index = TwitterManager

function TwitterManager.create(params)
	local twitterManager = {}             -- our new object
	setmetatable(twitterManager,TwitterManager)  -- make Account handle lookup
	
	-- Fill in the following fields from your Twitter app account
	twitterManager.consumerKey = params.consumerKey			-- key string goes here
	twitterManager.consumerSecret = params.consumerSecret	-- secret string goes here

	-- The web URL address below can be anything
	-- Twitter sends the webaddress with the token back to your app and the code strips out the token to use to authorise it
	--
	twitterManager.webURL = params.webURL

	-- Note: Once logged on, the access_token and access_token_secret should be saved so they can
	--	     be used for the next session without the user having to log in again.
	-- The following is returned after a successful authenications and log-in by the user
	--
	twitterManager.access_token = nil
	twitterManager.access_token_secret  = nil
	twitterManager.user_id = nil
	twitterManager.screen_name = nil

	-- Local variables used in the tweet
	twitterManager.postMessage = nil
	twitterManager.delegate = nil
	
	-- Display a message if there is not app keys set
	--
	if not twitterManager.consumerKey or not twitterManager.consumerSecret then
		-- Handle the response from showAlert dialog boxbox
		--
		local function onComplete( event )
			if event.index == 1 then
				system.openURL( "https://dev.twitter.com//" )
			end
		end

		native.showAlert( "Error", "To develop for Twitter, you need to get an API key and application secret. This is available from Twitter's website.",
			{ "Learn More", "Cancel" }, onComplete )
			
		return nil
	end

	
	-----------------------------------------------------------------------------------------
	-- Twitter Authorization Listener
	-----------------------------------------------------------------------------------------
	--
	twitterManager.listener = function(event)
		print("listener: ", event.url)
		local remain_open = true
		local url = event.url

		if url:find("oauth_token") and url:find(twitterManager.webURL) then
			url = url:sub(url:find("?") + 1, url:len())

			local authorize_response = twitterManager:responseToTable(url, {"=", "&"})
			remain_open = false

			local access_response = twitterManager:responseToTable(twitterManager:getAccessToken(authorize_response.oauth_token,
				authorize_response.oauth_verifier, twitterManager.twitter_request_token_secret,
				twitterManager.consumerKey, twitterManager.consumerSecret, "https://api.twitter.com/oauth/access_token"), {"=", "&"})
			
			twitterManager.access_token = access_response.oauth_token
			twitterManager.access_token_secret = access_response.oauth_token_secret
			twitterManager.user_id = access_response.user_id
			twitterManager.screen_name = access_response.screen_name
			
			print( "Tweeting" )
			
				-- API CALL:
			------------------------------
			--change the message posted
			local params = {}
			params[1] =
			{
				key = 'status',
				value = twitterManager.postMessage
			}
			
			request_response = twitterManager:makeRequest("https://api.twitter.com/1/statuses/update.json",
				params, twitterManager.consumerKey, twitterManager.access_token, twitterManager.consumerSecret,
					twitterManager.access_token_secret, "POST")
				
			--`print("req resp ",request_response)
			
			if(twitterManager.delegate and twitterManager.delegate.twitterSuccess) then 
				twitterManager.delegate.twitterSuccess()
			end

		elseif url:find(twitterManager.webURL) then
			-- Logon was canceled
			remain_open = false

			if(twitterManager.delegate and twitterManager.delegate.twitterCancel) then 
				twitterManager.delegate.twitterCancel()
			end
		end

		return remain_open
	end

	
	return twitterManager
	
end

-----------------------------------------------------------------------------------------
-- RESPONSE TO TABLE
--
-- Strips the token from the web address returned
-----------------------------------------------------------------------------------------
--
function TwitterManager:responseToTable(str, delimeters)
	local obj = {}

	while str:find(delimeters[1]) ~= nil do
		if #delimeters > 1 then
			local key_index = 1
			local val_index = str:find(delimeters[1])
			local key = str:sub(key_index, val_index - 1)
	
			str = str:sub((val_index + delimeters[1]:len()))
	
			local end_index
			local value
	
			if str:find(delimeters[2]) == nil then
				end_index = str:len()
				value = str
			else
				end_index = str:find(delimeters[2])
				value = str:sub(1, (end_index - 1))
				str = str:sub((end_index + delimeters[2]:len()), str:len())
			end
			obj[key] = value
			--print(key .. ":" .. value)		-- **debug
		else
	
			local val_index = str:find(delimeters[1])
			str = str:sub((val_index + delimeters[1]:len()))
	
			local end_index
			local value
	
			if str:find(delimeters[1]) == nil then
				end_index = str:len()
				value = str
			else
				end_index = str:find(delimeters[1])
				value = str:sub(1, (end_index - 1))
				str = str:sub(end_index, str:len())
			end
			
			obj[#obj + 1] = value
			--print(value)					-- **debug
		end
	end
	
	return obj
end

-----------------------------------------------------------------------------------------
-- Tweet
--
-- Sends the tweet. Authorizes if no access token
-----------------------------------------------------------------------------------------
--
function TwitterManager:tweet(msg,del)
	self.postMessage = msg
	self.delegate = del
	
	-- Check to see if we are authorized to tweet
	if not self.access_token then
		print( "Authorizing account" )
		
		if not self.consumerKey or not self.consumerSecret then
			-- Exit if no API keys set (avoids crashing app)
			if(self.delegate and self.delegate.twitterFailed) then 
				self.delegate.twitterFailed()
			end
			return
		end
		
		-- Need to authorize first
		--
		-- Get temporary token
		local twitter_request = (self:getRequestToken(self.consumerKey, self.webURL,
			"https://twitter.com/oauth/request_token", self.consumerSecret))
		self.twitter_request_token = twitter_request.token
		self.twitter_request_token_secret = twitter_request.token_secret
		print( "twitter_request_token: " ,self.twitter_request_token)
		print( "twitter_request_token_secret: " ,self.twitter_request_token_secret)
		if not self.twitter_request_token then
		
			-- No valid token received. Abort
			if(self.delegate and self.delegate.twitterFailed) then 
				self.delegate.twitterFailed()
			end

			return
		end
		
		-- Request the authorization
		native.showWebPopup(0, 0, 320, 480, "https://api.twitter.com/oauth/authorize?oauth_token="
			.. self.twitter_request_token, {urlRequest = self.listener})
	else
		print( "Tweeting" )
		
		------------------------------
		-- API CALL:
		------------------------------
		--change the message posted
		local params = {}
		params[1] =
		{
			key = 'status',
			value = self.postMessage
		}
		
--t-	request_response = oAuth.makeRequest("http://requestb.in/rllkvvrl", 

		self.request_response = self:makeRequest("https://api.twitter.com/1/statuses/update.json",
			params, self.consumerKey, self.access_token, self.consumerSecret, self.access_token_secret, "POST")
			
		--print("Tweet response: ",request_response)
		if(self.delegate and self.delegate.twitterSuccess) then 
			self.delegate.twitterSuccess()
		end
		
	end
end

-----------------------------------------------------------------------------------------
-- GET REQUEST TOKEN
-----------------------------------------------------------------------------------------
--
function TwitterManager:getRequestToken(consumer_key, token_ready_url, request_token_url, consumer_secret)
 
        local post_data = 
        {
                oauth_consumer_key = consumer_key,
                oauth_timestamp    = self:get_timestamp(),
                oauth_version      = '1.0',
                oauth_nonce        = self:get_nonce(),
                oauth_callback         = token_ready_url,
                oauth_signature_method = "HMAC-SHA1"
        }
    
    local post_data = self:oAuthSign(request_token_url, "POST", post_data, consumer_secret)
    
    local result = self:rawPostRequest(request_token_url, post_data)
	print("getRequestToken : Result = "..result)
    local token = result:match('oauth_token=([^&]+)')
    local token_secret = result:match('oauth_token_secret=([^&]+)')
        
        return 
        {
                token = token,
                token_secret = token_secret
        }
        
end

-----------------------------------------------------------------------------------------
-- GET ACCESS TOKEN
-----------------------------------------------------------------------------------------
--
function TwitterManager:getAccessToken(token, verifier, token_secret, consumer_key, consumer_secret, access_token_url)
            
    local post_data = 
        {
                oauth_consumer_key = consumer_key,
                oauth_timestamp    = self:get_timestamp(),
                oauth_version      = '1.0',
                oauth_nonce        = self:get_nonce(),
                oauth_token        = token,
                oauth_token_secret = token_secret,
                oauth_verifier     = verifier,
                oauth_signature_method = "HMAC-SHA1"
 
    }
    local post_data = self:oAuthSign(access_token_url, "POST", post_data, consumer_secret)
    local result = self:rawPostRequest(access_token_url, post_data)
    return result
end

-----------------------------------------------------------------------------------------
-- MAKE REQUEST
-----------------------------------------------------------------------------------------
--
function TwitterManager:makeRequest(url, body, consumer_key, token, consumer_secret, token_secret, method)
    
    local post_data = 
        {
                oauth_consumer_key = consumer_key,
                oauth_nonce        = self:get_nonce(),
                oauth_signature_method = "HMAC-SHA1",
                oauth_token        = token,
                oauth_timestamp    = self:get_timestamp(),
                oauth_version      = '1.0',
                oauth_token_secret = token_secret
    }
        for i=1, #body do
                post_data[body[i].key] = body[i].value
        end
    local post_data = self:oAuthSign(url, method, post_data, consumer_secret)
 
        local result
        
        if method == "POST" then
        result = self:rawPostRequest(url, post_data)
        else
        result = self:rawGetRequest(post_data)
        end
        
    return result
end

-----------------------------------------------------------------------------------------
-- OAUTH SIGN
-----------------------------------------------------------------------------------------
--
function TwitterManager:oAuthSign(url, method, args, consumer_secret)
 
    local token_secret = args.oauth_token_secret or ""
 
    args.oauth_token_secret = nil
 
        local keys_and_values = {}
 
        for key, val in pairs(args) do
                table.insert(keys_and_values, 
                {
                        key = self:encode_parameter(key),
                        val = self:encode_parameter(val)
                })
    end
 
    table.sort(keys_and_values, function(a,b)
        if a.key < b.key then
            return true
        elseif a.key > b.key then
            return false
        else
            return a.val < b.val
        end
    end)
    
    local key_value_pairs = {}
 
    for _, rec in pairs(keys_and_values) do
        table.insert(key_value_pairs, rec.key .. "=" .. rec.val)
    end
    
   local query_string_except_signature = table.concat(key_value_pairs, "&")
   
   local sign_base_string = method .. '&' .. self:encode_parameter(url) .. '&'
   		.. self:encode_parameter(query_string_except_signature)
 
   local key = self:encode_parameter(consumer_secret) .. '&' .. self:encode_parameter(token_secret)
   --print( "consumer_secret key: " .. consumer_secret )	-- **debug
   --print( "Encoded key: " .. key )						-- **debug
   local hmac_binary = self:sha1(sign_base_string, key, true)
 
   local hmac_b64 = mime.b64(hmac_binary)
   local query_string = query_string_except_signature .. '&oauth_signature=' .. self:encode_parameter(hmac_b64)
 
        if method == "GET" then
           return url .. "?" .. query_string
        else
                return query_string
        end
end

-----------------------------------------------------------------------------------------
-- ENCODE PARAMETER (URL_Encode)
-- Replaces unsafe URL characters with %hh (two hex characters)
-----------------------------------------------------------------------------------------
--
function TwitterManager:encode_parameter(str)
        return str:gsub('[^-%._~a-zA-Z0-9]',function(c)
                return string.format("%%%02x",c:byte()):upper()
        end)
end

-----------------------------------------------------------------------------------------
-- SHA 1
-----------------------------------------------------------------------------------------
--
function TwitterManager:sha1(str,key,binary)
        binary = binary or false
        return crypto.hmac(crypto.sha1,str,key,binary)
end

-----------------------------------------------------------------------------------------
-- GET NONCE
-----------------------------------------------------------------------------------------
--
function TwitterManager:get_nonce()
        return mime.b64(crypto.hmac(crypto.sha1,tostring(math.random()) .. "random"
        	.. tostring(os.time()),"keyyyy"))
end

-----------------------------------------------------------------------------------------
-- GET TIMESTAMP
-----------------------------------------------------------------------------------------
--
function TwitterManager:get_timestamp()
        return tostring(os.time() + 1)
end

-----------------------------------------------------------------------------------------
-- RAW GET REQUEST
-----------------------------------------------------------------------------------------
--
function TwitterManager:rawGetRequest(url)
        local r,c,h
        local response = {}
 
        r,c,h = http.request
        {
                url = url,
                sink = ltn12.sink.table(response)
        }
 
        return table.concat(response,"")
end

-----------------------------------------------------------------------------------------
-- RAW POST REQUEST
-----------------------------------------------------------------------------------------
--
function TwitterManager:rawPostRequest(url, rawdata)
 
        local r,c,h
        local response = {}
 
        r,c,h = http.request
        {
                url = url,
                method = "POST",
                headers = 
                {
                        ["Content-Type"] = "application/x-www-form-urlencoded", 
                        ["Content-Length"] = string.len(rawdata)
                },
                source = ltn12.source.string(rawdata),
                sink = ltn12.sink.table(response)
        }
 
        return table.concat(response,"")
end
