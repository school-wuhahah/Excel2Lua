--[[Notice: This lua config file is auto generate by Excel2Lua测试表.xls，don't modify it manually! --]]

local indexData = {
	id = 1, --id 
	Level = 2, --等级 
	rewards = 3, --奖励 
	desc = 4, --描述 
}

local data = {
	[1] = {[1]=1,[2]=2,[3]={1111,20,{2222,1}},[4]="1奖励",},
	[3] = {[1]=3,[2]=4,[3]={},[4]="3奖励",},
	[4] = {[1]=4,[2]=5,[3]={56},[4]="5奖励",},
	[5] = {[1]=5,[2]=6,[4]="6奖励",},
	[6] = {[1]=6,[2]=7,[3]={666,21,{2243,1},3},[4]="7奖励",},
	[7] = {[1]=7,[2]=8,[3]={56,56,66,89,50},[4]="8奖励",},
}

local mt = {}
mt.__index = function(t,k)
	if indexData[k] then
		return rawget(t,indexData[k]) 
	end
	return
end
mt.__newindex = function(t,k,v)
	error('do not edit config')
end
mt.__metatable = false
for _,v in pairs(data) do
	setmetatable(v,mt)
end

return data