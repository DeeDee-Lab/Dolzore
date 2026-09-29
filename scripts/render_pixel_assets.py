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
    # Draw at 240×360 and upscale exactly 2×. No text is rasterized into the cabinet.
    w,h=240,360
    im=Image.new("RGBA",(w,h),(0,0,0,0));d=ImageDraw.Draw(im)
    R=lambda a,c:d.rectangle(a,fill=c)
    L=lambda p,c,wid=1:d.line(p,fill=c,width=wid)

    ink="#1c1820"; deep="#2b2028"; burg="#7e3549"; red="#a9485b"
    gold="#d2a84a"; light="#f4dc76"; cream="#eadcb7"; chrome="#c9bea4"
    glass="#223a3e"; glass2="#315258"; teal="#4f8277"; speaker="#342b35"

    # Ground shadow / feet
    R((36,337,204,349),ink);R((48,348,78,357),ink);R((162,348,192,357),ink)

    # Strong classic stepped arch silhouette
    R((26,92,214,336),ink)
    R((32,70,208,336),ink)
    R((40,50,200,336),ink)
    R((52,34,188,336),ink)
    R((68,22,172,336),ink)
    R((84,14,156,336),ink)

    # Outer chrome/cream arch
    R((31,95,209,330),chrome)
    R((37,72,203,330),chrome)
    R((45,53,195,330),chrome)
    R((57,38,183,330),chrome)
    R((72,27,168,330),chrome)
    R((88,20,152,330),chrome)

    # Body arch
    R((37,98,203,326),burg)
    R((43,75,197,326),burg)
    R((51,56,189,326),burg)
    R((63,42,177,326),burg)
    R((78,32,162,326),burg)
    R((92,27,148,326),burg)

    # Illuminated side tubes — high contrast readable silhouette
    for x1,x2 in ((43,51),(189,197)):
        R((x1,82,x2,286),gold);R((x1+2,84,x2-2,284),light)
    for x1,x2,y1,y2 in ((53,62,58,88),(178,187,58,88),(64,74,44,62),(166,176,44,62),(79,91,33,47),(149,161,33,47)):
        R((x1,y1,x2,y2),gold);R((x1+2,y1+2,x2-2,y2-2),light)

    # Inner dark frame and glass record chamber
    R((61,66,179,166),ink)
    R((66,70,174,161),glass)
    R((72,76,168,155),glass2)
    # soft pixel reflections
    R((78,80,82,145),"#42676c");R((86,76,91,84),"#557b7d");R((158,82,163,145),"#172c30")

    # Record carousel: central spindle + several records
    for cx,cy,r,col in ((91,115,25,"#17131b"),(113,111,27,"#1b161d"),(137,116,25,"#17131b")):
        d.ellipse((cx-r,cy-r,cx+r,cy+r),fill=ink)
        d.ellipse((cx-r+4,cy-r+4,cx+r-4,cy+r-4),outline="#554756",width=2)
        d.ellipse((cx-5,cy-5,cx+5,cy+5),fill=col)
    d.ellipse((108,104,132,128),fill="#17131b")
    d.ellipse((113,109,127,123),fill="#d75c78")
    d.ellipse((118,114,122,118),fill=cream)
    # changer arm
    L(((139,83),(128,114)),chrome,4);L(((139,83),(147,79)),chrome,3)

    # Selection label window: recognizably jukebox-like rows
    R((52,172,188,225),ink);R((57,177,183,220),cream)
    for row in range(3):
        y=181+row*12
        for col in range(4):
            x=61+col*30
            R((x,y,x+25,y+8),"#fff5d4")
            R((x+2,y+2,x+6,y+3),red)
            R((x+8,y+2,x+22,y+3),"#8d7d68")
            R((x+8,y+5,x+19,y+6),"#b4a181")

    # Selector chrome strip
    R((47,230,193,255),ink);R((53,235,187,250),"#4b414a")
    for x in (61,91,121,151):
        R((x,238,x+18,248),chrome);R((x+4,241,x+14,245),"#6a5664")

    # Huge lower speaker grille is the key jukebox cue
    R((51,261,189,326),ink)
    R((58,268,182,318),speaker)
    for yy in range(270,318,5):
        for xx in range(60,182,6):
            col="#76636d" if (xx//6+yy//5)%2==0 else "#493d47"
            R((xx,yy,xx+2,yy+2),col)

    # Decorative bottom rails
    R((43,326,197,334),gold);R((49,326,191,330),light)
    R((39,107,44,286),red);R((196,107,201,286),red)

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
        # Architectural side plane. This is wall material, not a cast shadow.
        Poly(((x+w,y+12),(x+w+7,y+17),(x+w+7,y+h-1),(x+w,y+h+4)),side)
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
        # short contact shadow only; light source is upper-left
        R((x+5,y+h+4,x+w+6,y+h+6),"#557f4a")
        R((x+w+1,y+h+2,x+w+8,y+h+5),"#557f4a")
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
    im.save(OUT/"dolzore-town-game.png")

def characters():
    CELL_W,CELL_H=32,40
    sheet=Image.new("RGBA",(CELL_W*8,CELL_H*4),(0,0,0,0))

    # canonical character palettes from CHARACTER_BIBLE_V1
    chars=[
        dict(name="SORA",hair="#332A2D",skin="#D89C78",top="#58A394",accent="#EFE0B0",detail="#C96E63",bottom="#3D4B5C",shoe="#E9E0C9"),
        dict(name="MELO",hair="#7B453F",skin="#D99E78",top="#A85063",accent="#F0E2BF",detail="#E0AD45",bottom="#5C4051",shoe="#2B232B"),
        dict(name="YUZU",hair="#332F35",skin="#D7A178",top="#D4A747",accent="#F0E4BF",detail="#B85D62",bottom="#64745B",shoe="#29242A"),
        dict(name="PON",hair="#4B5A78",skin="#D69B74",top="#567EAD",accent="#DE7F45",detail="#B77A4D",bottom="#354765",shoe="#28222A"),
    ]

    outline="#26212A"
    eye="#38282C"
    mouth="#9C6658"

    def rect(d,x,y,w,h,c):
        d.rectangle((x,y,x+w-1,y+h-1),fill=c)

    def px(d,x,y,c):
        d.point((x,y),fill=c)

    def hair_shape(d,row,direction,p):
        h=p["hair"]
        # common head outline mass
        rect(d,8,1,16,14,outline)
        if row==0: # SORA rounded hair + lifted forelock
            rect(d,9,2,14,10,h)
            rect(d,7,6,3,6,h);rect(d,22,5,3,6,h)
            rect(d,13,0,5,3,h);rect(d,17,1,5,2,h)
        elif row==1: # MELO asymmetric bob + longer left side
            rect(d,9,2,14,10,h)
            rect(d,6,6,4,9,h);rect(d,22,5,4,6,h)
            rect(d,7,12,4,5,h)
            rect(d,23,3,3,3,p["detail"]) # amber clip
        elif row==2: # YUZU wider wavy lower contour
            rect(d,9,2,14,10,h)
            rect(d,6,7,4,8,h);rect(d,22,7,4,8,h)
            px(d,7,4,h);px(d,24,3,h);px(d,5,11,h);px(d,26,10,h)
        else: # PON tousled angular hair
            rect(d,9,3,14,9,h)
            rect(d,6,5,5,5,h);rect(d,21,4,5,6,h)
            rect(d,11,1,6,3,h);rect(d,16,0,6,4,h);rect(d,22,2,3,3,h)

        # face/ back
        if direction=="down":
            rect(d,10,8,12,8,p["skin"])
            px(d,12,10,eye);px(d,19,10,eye)
            px(d,15,13,mouth);px(d,16,13,mouth)
        elif direction=="up":
            rect(d,10,9,12,6,h)
        elif direction=="left":
            rect(d,9,8,11,8,p["skin"])
            px(d,11,10,eye);px(d,10,13,mouth)
            rect(d,21,7,3,7,h)
        else:
            rect(d,12,8,11,8,p["skin"])
            px(d,20,10,eye);px(d,21,13,mouth)
            rect(d,8,7,3,7,h)

    def body_shape(d,row,direction,step,p):
        # torso outline and inner layer
        rect(d,7,16,18,14,outline)
        rect(d,9,17,14,12,p["top"])

        # collar / scarf / inner shirt
        if row==0:
            # SORA cream diagonal strap is the signature
            for i in range(11):
                x=10+i
                y=17+(i//3)
                px(d,x,y,p["accent"])
                if i<8: px(d,x,y+1,p["accent"])
            rect(d,21,24,5,5,p["detail"]) # coral pouch
        elif row==1:
            rect(d,11,17,10,4,p["accent"])
            rect(d,8,17,4,2,p["top"])
            rect(d,23,22,4,5,p["detail"]) # square record-sleeve bag edge
        elif row==2:
            rect(d,11,17,10,3,p["accent"])
            rect(d,9,26,14,3,p["bottom"])
            rect(d,7,23,4,6,p["accent"]) # notebook
            px(d,8,24,p["detail"]);px(d,8,27,p["detail"]) # bookmark
        else:
            rect(d,10,17,12,3,p["accent"]) # orange scarf
            rect(d,22,19,5,9,outline)
            rect(d,23,20,4,7,p["detail"]) # box backpack

        # arms
        arm_y=18 if direction in ("left","right") else 19
        if direction=="left":
            rect(d,5,arm_y,4,9,outline);rect(d,6,arm_y+1,3,7,p["skin"])
            rect(d,24,20,3,7,outline)
        elif direction=="right":
            rect(d,23,arm_y,4,9,outline);rect(d,23,arm_y+1,3,7,p["skin"])
            rect(d,5,20,3,7,outline)
        else:
            rect(d,5,19,4,9,outline);rect(d,6,20,3,7,p["skin"])
            rect(d,23,19,4,9,outline);rect(d,23,20,3,7,p["skin"])

        # legs / stance
        if step:
            rect(d,10,30,5,7,p["bottom"]);rect(d,18,31,5,6,p["bottom"])
            rect(d,9,37,6,3,p["shoe"]);rect(d,18,37,6,3,p["shoe"])
        else:
            rect(d,11,30,5,7,p["bottom"]);rect(d,17,30,5,7,p["bottom"])
            rect(d,10,37,6,3,p["shoe"]);rect(d,17,37,6,3,p["shoe"])

        # PON wider stance
        if row==3 and step:
            rect(d,8,37,7,3,p["shoe"]);rect(d,19,37,7,3,p["shoe"])

    def frame(row,col,direction,step,p):
        f=Image.new("RGBA",(CELL_W,CELL_H),(0,0,0,0))
        d=ImageDraw.Draw(f)
        hair_shape(d,row,direction,p)
        body_shape(d,row,direction,step,p)
        sheet.alpha_composite(f,(col*CELL_W,row*CELL_H))

    frames=[("down",0),("down",1),("left",0),("left",1),("right",0),("right",1),("up",0),("up",1)]
    for row,p in enumerate(chars):
        for col,(direction,step) in enumerate(frames):
            frame(row,col,direction,step,p)

    sheet.save(OUT/"dolzore-characters.png",OUT/"dolzore-first-town.png")
town_game();characters()


def first_town():
    W,H=1536,1152
    im=Image.new("RGB",(W,H),"#78b866")
    d=ImageDraw.Draw(im)
    R=lambda a,c:d.rectangle(a,fill=c)
    L=lambda p,c,w=1:d.line(p,fill=c,width=w)
    Poly=lambda p,c:d.polygon(p,fill=c)

    # palette
    grass="#79b966"; grass2="#66a956"; grass3="#4e884b"
    road="#9f9978"; road2="#8a8569"; curb="#d2c8a0"; curb2="#b7ad87"
    ink="#2d2831"; shadow="#587f4d"
    cream="#eadfb8"; teal="#72ad99"; tealSide="#548777"
    rose="#cf7882"; roseSide="#a85b66"; ochre="#d9b36b"; ochreSide="#b58d55"
    blue="#6b91b0"; blueSide="#4c6d88"; olive="#89996a"; oliveSide="#687650"
    window="#9fd4cf"; window2="#d7eee3"; brown="#6d4e39"; gold="#deb64f"

    # subtle ground texture
    R((0,0,W,H),grass)
    for x in range(12,W,19):
        for y in range(12,H,19):
            if ((x*7+y*3)//19)%11==0:
                d.point((x,y),fill=grass2)
                if (x+y)%3==0:d.point((x+1,y),fill="#92c97b")

    # Riverside band
    R((0,905,W,1151),"#5fa5b2")
    R((0,897,W,910),"#d4c89b")
    for x in range(0,W,24):
        R((x,900,min(x+12,W),904),"#a9a07c")
    for x in range(18,W,64):
        L(((x,936),(x+18,934)),"#85c6cf",2)
        L(((x+10,1010),(x+34,1008)),"#4e8797",2)

    # riverbank greens / reeds
    R((0,850,W,897),grass2)
    for x in range(10,W,37):
        L(((x,888),(x+2,871)),grass3,2)
        L(((x+5,891),(x+7,875)),grass3,2)

    # Residential hill plateau
    R((0,0,650,380),"#82bf6b")
    R((0,370,650,390),"#6a9d57")
    for x in range(0,650,24):
        R((x,376,min(x+15,650),381),"#557b49")

    # roads: loops
    # main east-west
    R((120,430,1450,560),road)
    R((120,420,1450,432),curb)
    R((120,560,1450,572),curb)
    # residential descent
    R((470,240,590,760),road)
    R((458,240,470,760),curb)
    R((590,240,602,760),curb)
    # civic loop
    R((250,680,1050,790),road)
    R((250,668,1050,680),curb)
    R((250,790,1050,802),curb)
    # station road
    R((1040,500,1160,940),road)
    R((1028,500,1040,940),curb)
    R((1160,500,1172,940),curb)
    # riverside road
    R((180,790,1320,875),road)
    R((180,778,1320,790),curb)
    R((180,875,1320,887),curb)
    # market lane
    R((930,300,1010,760),road2)
    R((918,300,930,760),curb)
    R((1010,300,1022,760),curb)

    # lane markings
    for x in range(150,1430,90): R((x,492,x+42,497),"#e8ddb1")
    for y in range(270,730,74): R((528,y,533,y+34),"#e8ddb1")
    for x in range(280,1030,80): R((x,733,x+34,738),"#e8ddb1")
    for y in range(530,920,78): R((1098,y,1103,y+36),"#e8ddb1")

    # bridge
    R((760,875,930,930),"#81664c")
    R((768,881,922,924),"#c49a69")
    for x in range(775,920,24): R((x,884,x+8,921),"#a87d55")
    R((760,874,930,880),ink);R((760,924,930,930),ink)

    def building(x,y,w,h,front,side,roof):
        # contact shadow only
        R((x+8,y+h+5,x+w+10,y+h+9),shadow)
        R((x+w+1,y+18,x+w+9,y+h+4),side)
        R((x,y+18,x+w,y+h+5),ink)
        R((x+3,y+21,x+w-3,y+h+2),front)
        # roof plane
        Poly(((x-7,y+20),(x+18,y),(x+w-25,y),(x+w+8,y+20),(x+w,y+28),(x,y+28)),ink)
        Poly(((x+1,y+18),(x+20,y+5),(x+w-27,y+5),(x+w,y+18),(x+w-1,y+23),(x+2,y+23)),roof)
        # windows
        R((x+14,y+42,x+43,y+69),ink);R((x+17,y+45,x+40,y+66),window)
        R((x+w-46,y+42,x+w-17,y+69),ink);R((x+w-43,y+45,x+w-20,y+66),window2)
        # door
        dx=x+w//2-12
        R((dx,y+h-34,dx+24,y+h+3),ink);R((dx+3,y+h-31,dx+21,y+h+3),"#554451")
        R((dx+17,y+h-16,dx+19,y+h-13),gold)
        return {"door":(dx+12,y+h+5),"rect":(x,y,x+w+9,y+h+9)}

    def tree(x,y,scale=1.0):
        tr=int(6*scale)
        R((x-tr//2,y,x+tr//2,y+int(28*scale)),brown)
        blobs=[(-14,-12,16,16),(1,-18,18,17),(-4,-30,17,18),(-19,-4,14,14),(12,-6,14,14)]
        for dx,dy,bw,bh in blobs:
            R((x+int(dx*scale)-2,y+int(dy*scale)-2,x+int((dx+bw)*scale)+2,y+int((dy+bh)*scale)+2),"#2e6747")
            R((x+int(dx*scale),y+int(dy*scale),x+int((dx+bw)*scale),y+int((dy+bh)*scale)),"#438b58")
        R((x-int(3*scale),y-int(27*scale),x+int(8*scale),y-int(21*scale)),"#67ac69")

    def lamp(x,y):
        R((x,y,x+3,y+34),"#38464c");R((x-6,y-2,x+9,y+2),"#38464c")
        R((x-3,y-10,x+6,y-2),gold);R((x-1,y-8,x+4,y-4),"#fff0a5")

    # Residential Hill
    home=building(115,105,150,96,ochre,ochreSide,"#5f6e67")
    building(315,90,135,92,"#d49b72","#aa7656","#6d5f68")
    building(92,245,122,82,"#9eb779","#74895a","#596b5c")
    building(285,230,145,88,"#7ba9c0","#587d92","#4f596b")
    # water tower landmark
    R((535,92,600,140),ink);R((541,98,594,135),blue)
    R((548,140,554,205),ink);R((581,140,587,205),ink)
    R((540,203,596,210),ink)
    # small park
    for t in ((55,165,1.0),(62,300,.9),(448,120,1.1),(430,320,.9)): tree(*t)

    # Main Street buildings
    bar=building(610,330,180,105,rose,roseSide,"#6f3546")
    cafe=building(820,345,145,94,ochre,ochreSide,"#616e65")
    store=building(1080,330,160,100,teal,tealSide,"#475c58")
    service=building(1265,350,140,92,blue,blueSide,"#4b5364")
    # central plaza
    R((620,575,900,655),"#d8cc9f")
    for x in range(640,880,46): R((x,590,x+28,596),"#b9ac84")
    R((735,605,790,612),brown);R((744,613,750,635),brown);R((780,613,786,635),brown)

    # Market / workshop
    workshop=building(1015,195,165,96,"#a9836f","#7c604f","#4c5057")
    secondhand=building(1210,200,145,92,"#8ca57a","#697c5c","#57604f")
    building(1360,245,125,80,"#d4a875","#ad845a","#6e5960")
    # stalls
    for x,c in ((1045,"#bd5966"),(1125,"#5b8e78"),(1205,"#d0a84e")):
        R((x,615,x+62,652),cream);R((x,605,x+62,618),c)
        R((x+5,652,x+9,676),brown);R((x+53,652,x+57,676),brown)

    # Civic / JOURNAL
    journal=building(320,600,190,110,teal,tealSide,"#4c4152")
    building(540,625,145,90,"#c2a276","#9d7c57","#5e5b63")
    # civic map kiosk
    R((715,650,770,720),ink);R((720,655,765,710),cream)
    R((726,662,758,667),rose);R((726,675,752,680),blue);R((726,688,760,693),teal)

    # Riverside structures
    building(130,775,145,88,"#a7b47a","#7e895a","#586052")
    building(1020,785,155,94,"#c7907e","#9e695c","#60536a")
    # big landmark tree
    tree(560,842,1.7)
    # fishing pier
    R((370,880,465,925),brown);R((377,886,458,920),"#a17750")
    for x in range(382,458,18): R((x,889,x+5,917),"#7e5c40")

    # Station / East Gate
    station=building(1220,690,220,125,"#8da7b3","#667b85","#485260")
    # station clock tower
    R((1315,620,1365,690),ink);R((1321,626,1359,686),"#d2c59e")
    d.ellipse((1328,636,1352,660),fill="#f3e7c5",outline=ink,width=3)
    L(((1340,648),(1340,641)),ink,2);L(((1340,648),(1347,653)),ink,2)
    # east gate road continuation
    R((1450,430,1535,560),road);R((1450,420,1535,432),curb);R((1450,560,1535,572),curb)

    # Back alley pocket
    R((790,205,900,320),"#659f58")
    R((807,236,830,300),ink);R((811,240,826,296),"#426d8d") # vending machine
    R((840,282,892,288),brown);R((846,289,851,310),brown);R((883,289,888,310),brown)
    # graffiti-ish pixel marks
    for x,y,c in ((795,219,rose),(803,214,gold),(812,220,blue),(821,214,teal)):
        R((x,y,x+6,y+3),c)

    # trees around districts
    for t in [(45,80,1),(70,390,1),(220,360,1),(690,260,1),(900,270,1),(1460,380,1),
              (260,850,1),(690,850,1),(900,850,1),(1180,900,1),(1450,860,1)]:
        tree(*t)

    # lamps
    for x,y in [(180,410),(410,410),(650,410),(900,410),(1130,410),(1380,410),
                (300,660),(520,660),(810,660),(980,660),(1210,770)]:
        lamp(x,y)

    # parked cars
    def car(x,y,color):
        R((x,y+8,x+52,y+28),ink);R((x+4,y+5,x+47,y+24),color);R((x+13,y,x+37,y+10),color)
        R((x+16,y+2,x+34,y+8),window);R((x+7,y+24,x+14,y+31),ink);R((x+39,y+24,x+46,y+31),ink)
    car(280,468,rose);car(1180,500,blue);car(850,720,"#7f936d")

    # district boundary details / drains
    for x,y in [(210,565),(470,565),(740,565),(1010,565),(1280,565),(420,795),(840,795),(1110,875)]:
        R((x,y,x+22,y+5),"#56554e")
        for gx in range(x+3,x+22,5): L(((gx,y+1),(gx,y+4)),"#999274")

    im.save(OUT/"dolzore-first-town.png")

print("rendered",OUT/"dolzore-town.png",OUT/"dolzore-jukebox.png",OUT/"dolzore-town-game.png",OUT/"dolzore-characters.png")
