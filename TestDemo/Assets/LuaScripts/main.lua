
require("utils.tableUtil")

local function main()
    local xlsdata = require("dataConfig.conf_test_xls")
    local xlsxdata = require("dataConfig.conf_test_xlsx")
    print("=================================xlsdata bgn========================")
    for _, data in pairs(xlsdata) do
        print(string.format( "id : %s, levl : %s, desc  : %s, rewards : %s ", data.id, data.Level, data.desc, table.dump(data.rewards, nil, 3)))
    end
    print("=================================xlsdata end========================")

    print("=================================xlsxdata bgn========================")
    for _, data in pairs(xlsxdata) do
        print(string.format( "id : %s, levl : %s, desc  : %s, rewards : %s ", data.id, data.Level, data.desc, table.dump(data.rewards, nil, 3)))
    end
    print("=================================xlsxdata end========================")
end

local function logtraceback(msg)
    local tracemsg = debug.traceback()
    print("error: " .. tostring(msg) .. "\n" .. tracemsg)
    return msg
end

local ret, msg = xpcall(main, logtraceback)
if not ret then
    error("\n" .. "lua error msg:" .. "\n" .. "\t" .. msg)
end
