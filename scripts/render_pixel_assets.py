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

def town_game():
    W,H=480,270
    im=Image.new("RGB",(W,H),"#90d3dc"); d=ImageDraw.Draw(im)
    R=lambda a,c:d.rectangle(a,fill=c)
    L=lambda p,c,w=1:d.line(p,fill=c,width=w)
    Poly=lambda p,c:d.polygon(p,fill=c)

    # sky strip and grass
    R((0,0,W,34),"#9adbe2")
    R((0,34,W,H),"#78b45f")
    for x in range(0,W,9):
        for y in range(42,H,9):
            if (x*3+y)//9%7==0:
                d.point((x+2,y+1),fill="#5f9952")
                if (x+y)%4==0:d.point((x+3,y+1),fill="#90c775")

    # road network (walkable)
    R((0,126,W,198),"#9b9675")
    R((304,104,372,H),"#9b9675")
    R((0,118,W,126),"#cfc79b")
    R((0,198,W,207),"#cfc79b")
    R((296,104,304,H),"#cfc79b")
    R((372,104,380,H),"#cfc79b")
    for x in range(12,W,54):R((x,160,x+24,163),"#e8ddb0")
    for y in range(214,H,34):R((336,y,339,y+15),"#e8ddb0")
    # curb pixels
    for x in range(0,W,12):
        R((x,119,min(x+6,W),121),"#b8ad84")
        R((x,202,min(x+6,W),204),"#b8ad84")

    def tree(x,y):
        R((x+7,y+18,x+11,y+34),"#6b4d37")
        for a in ((x+2,y+9,x+16,y+22),(x,y+14,x+18,y+26),(x+5,y+3,x+14,y+17)):
            R((a[0]-1,a[1]-1,a[2]+1,a[3]+1),"#2d6747")
            R(a,"#458c58")
        R((x+6,y+6,x+12,y+10),"#69aa67")
    for p in ((10,57),(153,81),(451,72),(398,219),(18,224)):tree(*p)

    def building(x,y,w,h,front,side,roof):
        # back shadow
        Poly(((x+7,y+8),(x+w+10,y+8),(x+w+16,y+14),(x+w+16,y+h+7),(x+14,y+h+7),(x+7,y+h)), "#413744")
        # side plane
        Poly(((x+w,y+12),(x+w+8,y+18),(x+w+8,y+h-2),(x+w,y+h+5)),side)
        # front
        R((x,y+12,x+w,y+h+4),"#2e2932")
        R((x+2,y+14,x+w-2,y+h+2),front)
        # roof 3/4
        Poly(((x-3,y+13),(x+9,y+2),(x+w-15,y+2),(x+w+4,y+13),(x+w,y+18),(x,y+18)),"#2e2932")
        Poly(((x+1,y+12),(x+10,y+5),(x+w-17,y+5),(x+w,y+12),(x+w-1,y+15),(x+2,y+15)),roof)
        # awning and windows
        for ix in range(x+8,x+w-8,10):
            R((ix,y+34,min(ix+6,x+w-8),y+38),"#f1e0ad" if ((ix-x)//10)%2==0 else front)
        R((x+10,y+43,x+34,y+65),"#2b2630");R((x+12,y+45,x+32,y+63),"#9fd4cf")
        R((x+w-37,y+43,x+w-13,y+65),"#2b2630");R((x+w-35,y+45,x+w-15,y+63),"#cfe8da")
        # door
        dx=x+w//2-10
        R((dx,y+h-25,dx+20,y+h+2),"#2b2630");R((dx+2,y+h-23,dx+18,y+h+2),"#554451")
        R((dx+14,y+h-12,dx+16,y+h-10),"#d1a94a")
        return (dx+2,y+h-8,16,12)

    cafe_door=building(34,44,112,70,"#d6b16c","#b68e59","#596c68")
    journal_door=building(174,38,124,80,"#70ac98","#4d8275","#493e4d")
    music_door=building(336,35,116,84,"#d9848a","#b85e69","#864254")

    # sidewalk furniture, parked car, postbox, vending machine
    R((226,105,272,110),"#71523c");R((232,99,266,103),"#8a6648");R((232,110,235,121),"#4b3b32");R((263,110,266,121),"#4b3b32")
    R((286,85,298,112),"#355c7a");R((288,87,296,110),"#4d7795");R((289,90,295,97),"#ece0b8");R((291,101,294,105),"#263a4b")
    R((390,93,408,118),"#2e2932");R((392,95,406,116),"#e6d7aa");R((394,98,404,101),"#bb5966");R((394,105,402,107),"#4f7694");R((394,111,403,113),"#4d8375")
    # car
    R((78,145,120,164),"#29252c");R((81,141,116,160),"#b95764");R((90,137,108,146),"#b95764")
    R((92,139,106,144),"#9fd4cf");R((84,159,91,166),"#29252c");R((108,159,115,166),"#29252c")
    # crosswalk
    for xx in range(309,367,10):R((xx,180,xx+6,194),"#e8ddb0")
    # lamp posts
    for x,y in ((158,105),(319,114),(432,109)):
        R((x,y,x+2,y+30),"#36434a");R((x-4,y-1,x+6,y+3),"#36434a");R((x-2,y-6,x+4,y),"#e2c45f")
    # drain grates
    for x,y in ((132,199),(405,199),(346,232)):
        R((x,y,x+14,y+4),"#55544c")
        for xx in range(x+2,x+14,3):L(((xx,y+1),(xx,y+3)),"#999274")
    # small flowers
    for x,y,c in ((28,104,"#e5b04f"),(151,107,"#c95e78"),(457,113,"#e8d66a"),(414,245,"#d66a7f"),(42,243,"#ead159")):
        d.point((x,y),fill=c);d.point((x+1,y),fill=c);d.point((x,y+1),fill="#3e7b4b")
    im.resize((960,540),Image.Resampling.NEAREST).save(OUT/"dolzore-town-game.png")

def characters():
    cell_w,cell_h=16,24
    sheet=Image.new("RGBA",(cell_w*8,cell_h*4),(0,0,0,0))

    chars=[
        # hair, skin, top, accent, bottom, shoes
        ("#2d2832","#d9a078","#4d9a8e","#e7d8a5","#344153","#241f28"), # Sora
        ("#6d3d3b","#d8a078","#a44f5c","#e2bd62","#60434c","#271f27"), # Melo
        ("#332e36","#d7a177","#d3a443","#6a8d78","#4d5a50","#272229"), # Yuzu
        ("#31405e","#d59c73","#5379a8","#d87f4a","#43516c","#231f27"), # Pon
    ]

    def px(d,x,y,w,h,c): d.rectangle((x,y,x+w-1,y+h-1),fill=c)

    def draw_frame(row,col,direction,step,pal):
        hair,skin,top,accent,bottom,shoes=pal
        ox=col*cell_w;oy=row*cell_h
        imf=Image.new("RGBA",(cell_w,cell_h),(0,0,0,0));d=ImageDraw.Draw(imf)

        # shadow is drawn at runtime, keep sprite clean
        if direction=="down":
            px(d,4,1,8,7,"#25212a");px(d,5,2,6,6,hair);px(d,5,5,6,4,skin)
            px(d,6,6,1,1,"#3a2a2d");px(d,9,6,1,1,"#3a2a2d")
            px(d,4,9,8,8,"#25212a");px(d,5,10,6,6,top)
            px(d,3,11,2,6,skin);px(d,11,11,2,6,skin)
            # character-specific accent
            if row==0: px(d,10,10,2,7,accent);px(d,9,13,2,2,"#6f5b45")
            elif row==1: px(d,5,9,6,2,accent);px(d,3,8,2,3,"#d3a443");px(d,11,8,2,3,"#d3a443")
            elif row==2: px(d,6,6,1,1,"#eee0c0");px(d,9,6,1,1,"#eee0c0");px(d,5,12,6,2,accent)
            elif row==3: px(d,5,9,6,2,accent);px(d,8,10,2,6,"#b65e47")
            lx=5+(1 if step else 0);rx=9-(1 if step else 0)
            px(d,lx,17,2,5,bottom);px(d,rx,17,2,5,bottom);px(d,lx,22,2,2,shoes);px(d,rx,22,2,2,shoes)
        elif direction=="up":
            px(d,4,1,8,8,"#25212a");px(d,5,2,6,7,hair)
            px(d,4,9,8,8,"#25212a");px(d,5,10,6,6,top)
            if row==0:px(d,10,10,2,7,accent)
            elif row==1:px(d,5,9,6,2,accent)
            elif row==2:px(d,5,12,6,2,accent)
            elif row==3:px(d,5,9,6,2,accent);px(d,8,10,2,6,"#b65e47")
            px(d,3,11,2,6,skin);px(d,11,11,2,6,skin)
            lx=5+(1 if step else 0);rx=9-(1 if step else 0)
            px(d,lx,17,2,5,bottom);px(d,rx,17,2,5,bottom);px(d,lx,22,2,2,shoes);px(d,rx,22,2,2,shoes)
        else:
            left=direction=="left"
            px(d,4,1,8,7,"#25212a");px(d,5 if left else 4,2,6,6,hair);px(d,6 if left else 5,5,5,4,skin)
            eye_x=6 if left else 9;px(d,eye_x,6,1,1,"#3a2a2d")
            px(d,4,9,8,8,"#25212a");px(d,5,10,6,6,top)
            arm_x=3 if left else 11;px(d,arm_x,11,2,6,skin)
            if row==0:px(d,10 if left else 4,10,2,7,accent)
            elif row==1:px(d,5,9,6,2,accent)
            elif row==2:px(d,6,12,5,2,accent)
            elif row==3:px(d,5,9,6,2,accent);px(d,8,10,2,6,"#b65e47")
            if step:
                px(d,5,17,2,5,bottom);px(d,9,18,2,4,bottom);px(d,5,22,2,2,shoes);px(d,9,22,2,2,shoes)
            else:
                px(d,6,17,2,5,bottom);px(d,9,17,2,5,bottom);px(d,6,22,2,2,shoes);px(d,9,22,2,2,shoes)

        sheet.alpha_composite(imf,(ox,oy))

    for row,pal in enumerate(chars):
        frames=[("down",0),("down",1),("left",0),("left",1),("right",0),("right",1),("up",0),("up",1)]
        for col,(direction,step) in enumerate(frames):draw_frame(row,col,direction,step,pal)
    sheet.save(OUT/"dolzore-characters.png")

town_game();characters()

print("rendered",OUT/"dolzore-town.png",OUT/"dolzore-jukebox.png",OUT/"dolzore-town-game.png",OUT/"dolzore-characters.png")
