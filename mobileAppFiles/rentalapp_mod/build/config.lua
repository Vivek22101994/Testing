print(system.getInfo("model"));
if system.getInfo("model") == "Droid" then   
print("Droid")
        application =
        {
                content =
                {
                        --zoom
                        width = 640,
                        height = 960,
                        fps = 60,
                        scale = "letterbox",
                        
                        imageSuffix =
						{
							["@2"] = 2,
						}
                }
        }         
elseif system.getInfo("model") ~= "iPhone4" and system.getInfo("model") ~= "iPad" and system.getInfo("platformName") ~= "Android" then   
print("iPhone")
        application =
        {
                content =
                {
                        --zoom
                        width = 640,
                        height = 960,
                        fps = 60,
                        scale = "letterbox",
                        
                        imageSuffix =
						{
							["@2"] = 2,
						}
                }
        }         
else
print("others")
	application =
        {
                content =
                {
                        width = 640,
                        height = 960,
                        fps = 60,
                        scale = "letterbox",
                        
                        imageSuffix =
						{
							["@2"] = 1,
						}
                }
        } 
end