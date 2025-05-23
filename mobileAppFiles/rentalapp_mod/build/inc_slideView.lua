-- slideView.lua
-- 
-- Version 1.0 
--
 
module(..., package.seeall)
 
local screenW = display.contentWidth;
local screenH = display.contentHeight;
local viewableScreenW, viewableScreenH = display.viewableContentWidth, display.viewableContentHeight
local screenOffsetW, screenOffsetH = display.contentWidth -  display.viewableContentWidth, display.contentHeight - display.viewableContentHeight
 
local imgNum = nil
local images = nil
local touchListener, nextImage, prevImage, cancelMove, initImage
local background
local imageNumberText
 
function new( imageSet, slideBackground, top, bottom )  
        local pad = 50
        local top = top or 0 
        local bottom = bottom or 0
 
        local g = display.newGroup()
                
        if slideBackground then
                background = display.newImage(slideBackground, 0, 0, true)
        else
                background = display.newRect( 0, 0, screenW, screenH-(top+bottom) )
                background:setFillColor(0, 0, 0)
        end
        g:insert(background)
        
        images = {}
        for i = 1,#imageSet do
                local p = display.newImage(imageSet[i], system.TemporaryDirectory)
                local h = viewableScreenH-(top+bottom)
                if p.width > viewableScreenW or p.height > h then
                        if p.width/viewableScreenW > p.height/h then 
                                        p.xScale = viewableScreenW/p.width
                                        p.yScale = viewableScreenW/p.width
                        else
                                        p.xScale = h/p.height
                                        p.yScale = h/p.height
                        end              
                end
                g:insert(p)
            
                if (i > 1) then
                        p.x = screenW*1.5 + pad -- all images offscreen except the first one
                else 
                        p.x = screenW*.5
                end
                
                p.y = h*.5
 
                images[i] = p
        end
        
        local defaultString = "1 of " .. #images
        local navBar = display.newGroup()
        g:insert(navBar)
        -- local navBarGraphic = display.newImageRect("img_slideView_navBar.png", 648, 62 )
        -- navBar:insert(navBarGraphic)
        -- navBarGraphic.x = viewableScreenW*.5
        -- navBarGraphic.y = 31

        imageNumberText = display.newText(defaultString, 0, 0, native.systemFontBold, 30);
        imageNumberText:setTextColor(255, 255, 255)
        navBar:insert(imageNumberText)
        imageNumberText.x = 648*.5
        imageNumberText.y = 360+190
        
 
        imgNum = 1
        
        g.x = 0
        g.y = top;-- + display.screenOriginY
                        
        function touchListener (self, touch) 
                local phase = touch.phase
                --print("slides", phase)
                if ( phase == "began" ) then
            -- Subsequent touch events will target button even if they are outside the stageBounds of button
            display.getCurrentStage():setFocus( self )
            self.isFocus = true
 
                        startPos = touch.x
                        prevPos = touch.x
                        
                                                                        
        elseif( self.isFocus ) then
        
                        if ( phase == "moved" ) then
                        
                                                
                                if tween then transition.cancel(tween) end
        
                                --print(imgNum)
                                
                                local delta = touch.x - prevPos
                                prevPos = touch.x
                                
                                images[imgNum].x = images[imgNum].x + delta
                                
                                if (images[imgNum-1]) then
                                        images[imgNum-1].x = images[imgNum-1].x + delta
                                end
                                
                                if (images[imgNum+1]) then
                                        images[imgNum+1].x = images[imgNum+1].x + delta
                                end
 
                        elseif ( phase == "ended" or phase == "cancelled" ) then
                                
                                dragDistance = touch.x - startPos
                                --print("dragDistance: " .. dragDistance)
                                
                                if (dragDistance < -40 and imgNum < #images) then
                                        nextImage()
                                elseif (dragDistance > 40 and imgNum > 1) then
                                        prevImage()
                                else
                                        cancelMove()
                                end
                                                                        
                                if ( phase == "cancelled" ) then                
                                        cancelMove()
                                end
 
                -- Allow touch events to be sent normally to the objects they "hit"
                display.getCurrentStage():setFocus( nil )
                self.isFocus = false
                                                                                                                
                        end
                end
                                        
                return true
                
        end
        
        function setSlideNumber()
                --print("setSlideNumber", imgNum .. " of " .. #images)
                imageNumberText.text = imgNum .. " of " .. #images
        end
        
        function cancelTween()
                if prevTween then 
                        transition.cancel(prevTween)
                end
                prevTween = tween 
        end
        
        function nextImage()
                tween = transition.to( images[imgNum], {time=400, x=(screenW*.5 + pad)*-1, transition=easing.outExpo } )
                tween = transition.to( images[imgNum+1], {time=400, x=screenW*.5, transition=easing.outExpo } )
                imgNum = imgNum + 1
                initImage(imgNum)
        end
        
        function prevImage()
                tween = transition.to( images[imgNum], {time=400, x=screenW*1.5+pad, transition=easing.outExpo } )
                tween = transition.to( images[imgNum-1], {time=400, x=screenW*.5, transition=easing.outExpo } )
                imgNum = imgNum - 1
                initImage(imgNum)
        end
        
        function cancelMove()
                tween = transition.to( images[imgNum], {time=400, x=screenW*.5, transition=easing.outExpo } )
                tween = transition.to( images[imgNum-1], {time=400, x=(screenW*.5 + pad)*-1, transition=easing.outExpo } )
                tween = transition.to( images[imgNum+1], {time=400, x=screenW*1.5+pad, transition=easing.outExpo } )
        end
        
        function initImage(num)
                if (num < #images) then
                        images[num+1].x = screenW*1.5 + pad                     
                end
                if (num > 1) then
                        images[num-1].x = (screenW*.5 + pad)*-1
                end
                setSlideNumber()
        end
 
        background.touch = touchListener
        background:addEventListener( "touch", background )
 
        ------------------------
        -- Define public methods
        
        function g:jumpToImage(num)
                local i
                --print("jumpToImage")
                --print("#images", #images)
                for i = 1, #images do
                        if i < num then
                                images[i].x = -screenW*.5;
                        elseif i > num then
                                images[i].x = screenW*1.5 + pad
                        else
                                images[i].x = screenW*.5 - pad
                        end
                end
                imgNum = num
                initImage(imgNum)
        end
 
        function g:cleanUp()
                --print("slides cleanUp")
                background:removeEventListener("touch", touchListener)
        end
        return g        
end