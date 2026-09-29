from PIL import Image, ImageDraw, ImageFont
from pathlib import Path

OUT=Path(__file__).resolve().parents[1]/"docs"/"assets"
OUT.mkdir(parents=True,exist_ok=True)

P={
"ink":"#2a2530","ink2":"#453a49","cloud":"#f7f0d8","grass":"#79b761","grass2":"#62a251","grass3":"#4b8547",
"road":"#9f9a76","road2":"#8b8666","side":"#cfc9a0","side2":"#b9b28b","cream":"#efe2b8","cream2":"#dac893",
"red":"#c0606c","red2":"#8d4050","pink":"#dc858c","teal":"#6faf9a","teal2":"#477c6e","blue":"#4f7397",
"blue2":"#38556f","gold":"#d9ad4b","gold2":"#b88735","brown":"#765641","brown2":"#543d31","white":"#fff8df",
"window":"#9fd6d2","window2":"#dff0df"
}

def font(size):
    for name in ("DejaVuSansMono-Bold.ttf","C:/Windows/Fonts/consolab.ttf"):
        try:return ImageFont.truetype(name,size)
        except:pass
    return None

def town():
    W,H=480,270
    im=Image.new("RGB",(W,H),"#8fd6dd"); d=ImageDraw.Draw(im)
    R=lambda a,c:d.rectangle(a,fill=c)
    L=lambda p,c,w=1:d.line(p,fill=c,width=w)
    Poly=lambda p,c:d.polygon(p,fill=c)

    R((0,0,W,45),"#9bdce3")
    for ox,oy in ((40,16),(160,9),(350,19)):
        R((ox,oy,ox+32,oy+7),P["cloud"]);R((ox+8,oy-5,ox+22,oy+1),P["cloud"]);R((ox+19,oy-2,ox+40,oy+5),P["cloud"])
    R((0,45,W,116),P["grass"])
    for x in range(0,W,12):
        for y in range(54,112,12):
            if (x+y)//12%3==0:R((x+2,y+2,x+4,y+4),P["grass2"])
    R((0,116,W,136),P["side"]);R((0,136,W,201),P["road"]);R((0,201,W,220),P["side"]);R((0,220,W,H),P["grass2"])
    for y in (120,128,205,213):L(((0,y),(W,y)),P["side2"])
    for x in range(-10,W,16):
        L(((x,116),(x+12,136)),P["side2"]);L(((x,201),(x+12,220)),P["side2"])
    for x in range(16,W,60):R((x,167,x+27,170),"#e7ddb1")
    for x in (96,300,424):
        R((x,194,x+14,198),"#59584d")
        for gx in range(x+2,x+13,3):L(((gx,195),(gx,197)),"#9d9878")

    def building(x,y,w,h,wall,side,roof,sign,signbg,door="left"):
        Poly(((x+8,y+10),(x+w+9,y+10),(x+w+16,y+17),(x+w+16,y+h+9),(x+15,y+h+9),(x+8,y+h+2)),"#4a404d")
        Poly(((x+w,y+12),(x+w+8,y+18),(x+w+8,y+h),(x+w,y+h+6)),P["ink"])
        Poly(((x+w+1,y+14),(x+w+6,y+19),(x+w+6,y+h-2),(x+w+1,y+h+3)),side)
        R((x,y+12,x+w,y+h+6),P["ink"]);R((x+2,y+14,x+w-2,y+h+4),wall)
        Poly(((x-3,y+14),(x+8,y+2),(x+w-16,y+2),(x+w+4,y+14),(x+w,y+18),(x,y+18)),P["ink"])
        Poly(((x+1,y+13),(x+10,y+5),(x+w-17,y+5),(x+w,y+13),(x+w-1,y+15),(x+2,y+15)),roof)
        R((x+14,y+7,x+34,y+8),"#927177" if roof==P["red2"] else "#789180")
        for dx in range(x+44,x+w-20,10):d.point((dx,y+9),fill="#80656d" if roof==P["red2"] else "#617a70")
        sy=y+22;R((x+8,sy,x+w-8,sy+15),P["ink"]);R((x+10,sy+2,x+w-10,sy+13),signbg)
        f=font(8)
        if f:
            bb=d.textbbox((0,0),sign,font=f);tw=bb[2]-bb[0]
            d.text((x+(w-tw)//2,sy+2),sign,font=f,fill=P["white"])
        ay=sy+18
        for ix in range(x+8,x+w-8,8):R((ix,ay,min(ix+5,x+w-8),ay+4),P["cream"] if ((ix-x)//8)%2==0 else P["red"])
        base=y+h+4
        if door=="left":dx=x+14;wx=x+w-36
        else:dx=x+w-31;wx=x+10
        R((dx,base-36,dx+22,base),P["ink"]);R((dx+2,base-34,dx+20,base),"#524352");R((dx+16,base-18,dx+18,base-16),P["gold"])
        R((wx,base-34,wx+26,base-10),P["ink"]);R((wx+2,base-32,wx+24,base-12),P["window"]);R((wx+4,base-30,wx+22,base-20),P["window2"])
        L(((wx+13,base-32),(wx+13,base-12)),"#739c95");L(((wx+2,base-21),(wx+24,base-21)),"#739c95");R((dx-3,base,dx+25,base+3),P["brown2"])

    building(46,62,105,52,"#e0bd76","#caa363","#61756f","CAFE","#824754","right")
    building(187,52,118,62,P["teal"],"#5a9482",P["ink2"],"JOURNAL","#3c5d57","left")
    building(334,48,112,66,P["pink"],"#b76671",P["red2"],"MUSIC","#7c3548","right")
    R((423,58,437,72),P["ink"]);R((425,60,435,70),P["gold"]);R((428,61,431,67),P["ink"]);R((431,65,435,68),P["ink"])
    R((310,82,326,108),P["brown2"]);R((312,84,324,105),P["cream"]);R((314,87,322,89),P["red"]);R((314,93,320,95),P["blue"]);R((315,99,322,101),P["teal2"])

    def tree(x,y,s=1):
        tr=max(3,int(5*s));R((x-tr//2,y,x+tr//2,y+int(23*s)),P["brown2"])
        for dx,dy,w,h in ((-10,-8,12,11),(2,-12,13,12),(-3,-21,13,12),(-13,-2,11,10),(9,-3,11,10)):
            R((x+int(dx*s)-1,y+int(dy*s)-1,x+int((dx+w)*s)+1,y+int((dy+h)*s)+1),"#2b6745")
            R((x+int(dx*s),y+int(dy*s),x+int((dx+w)*s),y+int((dy+h)*s)),"#3d8454")
        R((x-int(4*s),y-int(19*s),x+int(6*s),y-int(14*s)),"#5aa668")
    for t in ((20,93,1),(166,103,.9),(463,99,.95),(25,245,.85),(455,248,.9)):tree(*t)

    for x in (173,324):
        R((x,106,x+2,135),P["ink2"]);R((x-5,103,x+7,106),P["ink2"]);R((x-3,99,x+5,104),P["gold"]);R((x-1,100,x+3,103),P["white"])
    R((306,112,339,116),P["brown"]);R((306,105,339,109),P["brown"]);R((310,116,313,126),P["brown2"]);R((332,116,335,126),P["brown2"])
    R((159,91,173,116),P["ink"]);R((161,93,171,114),"#d0c9a5");R((163,95,169,100),P["red"])
    for yy in (104,108):R((163,yy,169,yy+1),P["blue"])

    def car(x,y,body):
        R((x,y+5,x+36,y+17),P["ink"]);R((x+3,y+3,x+30,y+15),body);R((x+9,y,x+25,y+7),body);R((x+11,y+1,x+23,y+5),P["window"])
        R((x+5,y+15,x+10,y+20),P["ink"]);R((x+27,y+15,x+32,y+20),P["ink"]);R((x+6,y+16,x+9,y+19),"#b8ad8b");R((x+28,y+16,x+31,y+19),"#b8ad8b")
    car(66,149,P["red"]);car(366,177,P["blue"])
    R((215,142,270,161),P["ink"]);R((218,139,267,158),"#9f554f");R((225,141,244,150),P["window"]);R((247,141,261,150),P["window"])
    R((220,157,228,164),P["ink"]);R((258,157,266,164),P["ink"])
    f=font(6)
    if f:d.text((231,151),"DOLZ",font=f,fill=P["cream"])
    for x in range(201,258,10):R((x,136,x+6,151),P["cream"])

    def npc(x,y,shirt):
        R((x+4,y,x+9,y+5),P["ink"]);R((x+5,y+1,x+8,y+4),"#d39a73");R((x+3,y+5,x+10,y+13),P["ink"]);R((x+5,y+6,x+8,y+12),shirt)
        R((x+4,y+13,x+5,y+18),P["ink"]);R((x+8,y+13,x+9,y+18),P["ink"])
    npc(286,166,P["blue"]);npc(123,122,P["red"]);npc(344,126,"#8e74a0");npc(52,221,P["gold2"])
    R((102,190,109,203),P["red2"]);R((99,194,112,199),P["red"]);R((102,187,109,192),P["gold2"])
    R((450,169,461,184),P["blue2"]);R((452,171,459,177),P["blue"]);R((453,173,458,174),P["cream"])
    R((386,102,408,125),P["ink"]);R((389,106,405,123),P["cream2"]);R((387,102,410,106),P["red2"])
    for yy,c in ((110,P["blue"]),(114,P["red"]),(118,P["teal2"])):R((392,yy,402,yy+2),c)

    for y in range(48,112,6):
        for x in range(0,W,8):
            if (x+y)//8%5==0:d.point((x,y),fill=P["grass3"])
    for y in range(140,198,5):
        for x in range(0,W,10):
            if (x+y)//5%4==0:d.point((x,y),fill=P["road2"])
    im.resize((960,540),Image.Resampling.NEAREST).save(OUT/"dolzore-town.png")

def jukebox():
    w,h=160,240
    im=Image.new("RGBA",(w,h),(0,0,0,0));d=ImageDraw.Draw(im)
    R=lambda a,c:d.rectangle(a,fill=c)
    # stepped silhouette
    R((15,8,145,235),"#17131b");R((20,28,140,224),"#17131b");R((28,18,132,230),"#17131b");R((40,10,120,234),"#17131b")
    # chrome / wood / glow
    R((27,48,133,210),"#d7c796");R((33,32,127,219),"#d7c796");R((43,19,117,226),"#d7c796")
    R((34,49,126,207),P["red2"]);R((40,34,120,215),P["red2"]);R((49,24,111,222),P["red2"])
    R((39,46,45,202),P["gold2"]);R((115,46,121,202),P["gold2"]);R((40,46,43,202),"#f0cc65");R((117,46,120,202),"#f0cc65")
    R((47,38,113,193),"#3e736c");R((50,34,110,197),"#4f877c")
    # glass chamber
    R((53,39,107,102),P["ink"]);R((56,42,104,99),"#24383e")
    R((58,28,102,39),P["ink"]);R((60,30,100,37),P["cream"])
    for x in range(61,100,5):d.point((x,31),fill="#ead36f")
    d.ellipse((65,51,95,81),fill="#0f0d11");d.ellipse((67,53,93,79),outline="#443a47",width=2);d.ellipse((76,62,84,70),fill=P["pink"])
    R((89,50,92,73),"#d7c9a7");R((86,51,92,54),"#d7c9a7")
    # song ticket
    R((49,106,111,137),P["ink"]);R((52,109,108,134),P["cream2"]);R((55,112,105,131),P["white"])
    # chrome selection strip
    R((43,143,117,162),"#241e27");R((47,147,113,158),"#4a4149")
    for x in (51,66,81,96):R((x,149,x+9,156),P["cream"]);R((x+2,151,x+7,154),"#715963")
    # message panel / controls / speaker
    R((48,167,112,185),P["ink"]);R((51,170,109,182),"#1f4740")
    for x in range(54,106,4):
        if x%8==0:d.point((x,172),fill="#79b899")
    for x,c in ((49,"#e9dfbe"),(66,"#74b99a"),(83,"#e9dfbe"),(100,"#e9dfbe")):
        R((x,190,x+12,202),P["ink"]);R((x+2,192,x+10,200),c)
    R((48,207,112,226),P["ink"]);R((51,210,109,223),"#302832")
    for yy in range(211,223,3):
        for xx in range(52,109,4):d.point((xx,yy),fill="#64555e")
    R((38,222,51,231),P["ink"]);R((109,222,122,231),P["ink"]);R((31,70,34,118),"#f1db86");R((126,70,129,118),"#f1db86")
    im.resize((480,720),Image.Resampling.NEAREST).save(OUT/"dolzore-jukebox.png")

town();jukebox()
print("rendered",OUT/"dolzore-town.png",OUT/"dolzore-jukebox.png")
