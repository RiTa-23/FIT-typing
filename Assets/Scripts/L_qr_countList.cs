using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_qr_countList : MonoBehaviour
{
    public List<List<int>> qr_countList = new List<List<int>>()
    {
        //              人  類  初  の人  工  衛  星 を 完  成    さ せ た 時  代   背    景    の 中    、 「 電    波 科 学    」 の 振    興    の た め 、 昭       和 2  9   年   、 本    学    の 前    身    と な る 福    岡    高    等    無 線    電    信    学    校    は 創    設    さ れ ま し た 。 昭       和 ３ ３  年   に 福    岡    電    波 学     園   を 創    設    し 、 福    岡    電    波 高    等    学    校   （  現    在    の 福    岡    工    業       大       学     附   属      城          東      高     等       学     校       ）  が 開      設      さ  れ  ま  し  た  。   そ  の  後  、  昭         和  ３  ５  年      に   福     岡       電      子  工      業          短     期   大      学     （   現      在      の 福       岡      工      業          大      学      短     期   大       学     部  ）   、 昭          和  ３  ８  年     に  福       岡     電      波   学      園     電      子  工       業         大       学     が    開     学     し  ま  し   た  。  昭          和  ４  １  年      、  福      岡      工      業          大      学      に  名      称          変      更      し  て  以  来      、 １   学      部  ６  学     科   の  工      業          系     単       科  大      学      と し  て  発       展     し   て  き ま  し  た   。 平       成      ９  年     に   情          報     工      学      部   を  開      設      し   、 電      子  ・  情         報       系     の   大      学      と  し  て の   基  盤     を   整         え   ま  し た  。  ま   た  、 平       成     １  ３   年      に 社      会      環       境          学      部  を  開     設      し   、  工     業           系     大       学     の   責     務   で  あ  る  廃      棄 物       処     理   等     の   環      境          問      題     に   人     文       ・  社     会      科   学     系      か   ら と  り  く   む  こ  と と  な  り   、  ３  学      部  ９  学      科 と  し  て  現       在     に   至     り   ま  す  。  現      在      、  約      ４  万      名     の   卒     業          生      を   輩      出         し   て  い  ま  す  。
        //new List<int>(){0,0,1,1,2,2,3,4,4,5,5,6,6,7,7,8,9,9,10,10,11,12,13,14,15,15,16,16,17,17,18,19,19,20,21,22,22,23,24,25,25,26,27,28,28,29,29,30,31,32,33,34,34,34,35,36,37,38,38,39,40,40,41,41,42,43,43,44,44,45,46,47,48,48,49,49,50,50,51,51,52,53,53,54,54,55,55,56,56,57,57,58,59,59,60,60,61,62,63,64,65,66,67,67,67,68,69,70,71,71,72,73,73,74,74,75,75,76,77,77,78,78,79,80,80,81,81,82,83,84,84,85,85,86,86,87,88,88,89,89,90,90,91,91,92,93,93,94,94,95,96,96,97,97,98,98,99,99,99,100,100,101,101,102,103,103,104,104,104,105,105,106,106,107,107,108,108,109,109,110,111,112,112,113,113,114,115,116,117,118,119,120,121,122,123,124,124,124,125,126,127,128,128,129,130,130,131,131,132,132,133,134,134,135,135,135,136,136,137,138,138,139,139,140,141,141,142,142,143,144,144,145,145,146,146,147,147,147,148,148,149,149,150,150,151,152,152,153,153,154,155,156,157,157,157,158,159,160,161,161,162,163,163,164,164,165,165,166,167,167,168,168,169,169,170,171,171,172,172,172,173,173,174,174,175,176,176,177,177,178,179,180,181,182,183,183,183,184,185,186,187,187,188,189,189,190,190,191,191,192,192,192,193,193,194,194,195,196,196,197,197,197,198,198,199,199,200,201,202,203,203,204,205,206,206,207,208,209,209,210,211,212,212,213,213,213,214,214,215,215,216,217,217,218,218,219,220,221,222,222,223,223,224,225,226,227,228,229,230,231,231,232,232,233,234,234,235,236,236,236,237,237,238,238,239,239,240,241,242,242,243,243,244,245,246,246,247,248,249,249,249,250,250,251,251,252,253,253,254,254,255,256,257,258,259,260,260,261,262,262,262,263,264,265,266,267,268,269,270,271,271,272,272,273,274,275,275,276,277,277,278,278,279,279,280,280,280,281,281,282,283,284,284,285,285,286,287,288,288,289,289,289,290,290,291,291,292,292,293,294,294,295,296,297,298,299,299,300,301,301,302,302,303,304,304,305,306,306,307,307,307,308,308,309,309,310,311,311,312,312,313,314,314,315,315,316,317,317,318,318,319,320,321,322,323,324,325,326,327,328,329,330,331,332,332,333,334,335,335,336,337,338,339,340,340,341,341,342,343,343,344,345,346,347,348,348,349,349,350,351,351,352,353,353,354,354,355,356,356,357,357,357,358,358,359,360,360,361,361,361,362,363,364,365,366,367},
       
        //              人類初の人工衛星を完成させた時代背景の中、「電波科学」の振興のため、昭和２９年、本学の前身となる福岡高等無線電信学校は創設されました。昭和３３年に福岡電波学園を創設し、福岡電波高等学校（現在の福岡工業大学附属城東高等学校）が開設されました。その後、昭和３５年に福岡電子工業短期大学（現在の福岡工業大学短期大学部）、昭和３８年に福岡電波学園電子工業大学が開学しました。昭和４１年、福岡工業大学に名称変更して以来、１学部６学科の工業系単科大学として発展してきました。平成９年に情報工学部を開設し、電子・情報系の大学としての基盤を整えました。また、平成１３年に社会環境学部を開設し、工業系大学の責務である廃棄物処理等の環境問題に人文・社会科学系からとりくむこととなり、３学部９学科として現在に至ります。現在、約４万名の卒業生を輩出しています。
        new List<int>(){2,2,2,1,2,2,2,2,1,2,2,1,1,1,1,2,2,2,1,2,1,1,2,1,1,2,1,1,2,2,1,1,1,1,3,1,1,1,2,1,2,2,1,2,2,1,1,1,2,2,2,2,1,2,2,2,2,2,1,2,2,1,1,1,1,1,1,3,1,1,1,2,1,2,2,2,1,2,2,1,2,2,1,1,2,2,2,1,2,2,2,2,1,2,2,1,2,2,2,3,2,2,1,2,3,2,2,2,2,2,1,1,2,2,1,1,1,1,1,1,1,1,1,1,3,1,1,1,2,1,2,2,2,1,2,3,2,1,2,2,1,2,2,1,2,2,2,3,2,2,2,1,2,2,1,1,1,3,1,1,1,2,1,2,2,2,1,2,2,2,1,2,3,2,2,1,2,2,1,1,1,1,1,3,1,1,1,2,1,2,2,2,3,2,2,1,2,3,2,2,1,1,1,2,1,1,2,1,1,2,1,1,2,3,2,2,1,2,2,1,1,1,2,2,1,1,1,1,1,1,1,2,2,1,2,1,3,2,2,2,1,1,2,2,1,1,2,1,1,3,2,2,1,2,2,1,1,1,1,1,2,1,3,1,1,1,1,1,1,1,1,2,2,1,1,2,1,2,2,2,3,2,1,1,2,2,1,1,2,3,2,2,2,1,2,1,1,1,1,2,1,2,2,1,2,1,2,3,2,2,1,2,2,1,2,2,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,2,1,1,1,1,2,2,1,2,1,1,1,1,2,2,1,2,1,2,2,1,2,3,2,1,2,3,1,1,1,1,1,1},
        //             学習ポートフォリオは、大学生活の中で履修した授業での学習目標の達成具合や課外活動の成果などを自ら記録し振り返ることによって、自分の成長を感じ取ったり、新たな課題を発見したりしながら、最終的には4年間の 学習成果として、どのようなことを学び、どのようなことができるようになったかを確認 できるシステムです。学習ポートフォリオの活用を通じて、自分の目標を定め、それに対する学期ごとの目標設定および振り返りを行うことで、学習への自己調整力を高めることができます。
        new List<int>(){2,3,1,1,1,1,1,1,1,1,1,2,2,2,2,1,2,1,1,3,1,1,2,3,1,1,2,3,2,3,1,2,2,1,2,1,1,2,2,2,1,2,1,1,1,1,3,1,1,2,1,1,1,2,1,1,1,1,1,1,1,1,1,2,1,2,3,1,2,1,1,1,1,1,1,2,1,1,1,2,1,2,2,1,1,1,1,1,1,1,1,2,3,2,1,1,1,2,2,1,2,3,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,2,3,1,1,1,1,1,1,1,1,2,2,1,2,1,1,1,1,2,1,2,3,1,2,1,1,1,1,1,2,1,1,2,1,1,1,1,2,3,2,2,1,1,1,1,1,2,1,1,3,1,1,1,1,1,2,3,1,1,1,1,3,2,3,1,2,1,1,1,1,1,1,1,1,1,1},
        //             本学では、実践型人材の育成を目指して、全学的にアクティブ・ラーニングを推進しています。本学におけるアクティブ・ラーニングは、学生の意見表明及び振り返りを基本的な要素とする授業・学習形態のことであり、具体的にはグループ学習、グループディスカッション、 体験学習、課題解決学習などを取り入れた授業を行います。講義形式の授業だけでなく、教員と学生の双方向性が確保された授業の中で、自分の意見を積極的に述べたり、学生同士で積極的にコミュニケーションを行ったりすることにより、授業理解が深まるとともに、実践型人材に欠かせない汎用的スキル（社会で役立つ力）を身に付けることができます。
        new List<int>(){2,2,1,1,1,2,2,2,2,2,1,2,2,1,1,1,1,1,1,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,1,1,1,1,1,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,1,2,3,2,2,1,1,1,2,1,1,1,2,2,1,2,1,1,1,1,2,3,1,2,3,2,2,1,1,1,1,1,1,1,1,2,2,1,1,1,1,1,1,2,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,3,1,1,2,2,2,2,3,1,1,1,1,1,1,1,1,2,3,1,3,1,1,1,1,2,1,2,2,1,2,3,1,1,1,1,1,1,3,2,1,2,2,1,2,2,2,2,1,2,1,1,1,1,2,3,1,2,1,1,1,2,1,1,2,1,2,3,2,1,1,1,1,1,1,2,2,2,1,1,2,3,2,1,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,1,1,1,1,1,1,1,2,3,1,2,1,2,1,1,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,1,2,2,2,1,1,1,1,2,2,1,2,1,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        //             課外活動は、学生相互が切磋琢磨することで貴重な人間関係を学び、情操豊かな人間性を育む場です。本学では、全学生で構成される学生自治会があり、その自治会組織の中に体育会本部と学術文化会が所属し、各クラブ・サークル、同好会、愛好会、合わせて５０団体が活動しています。
        new List<int>(){1,2,2,2,1,1,2,2,2,1,1,2,1,2,1,1,1,1,1,1,1,3,1,2,2,2,2,1,2,1,1,3,2,2,1,1,2,2,2,1,3,1,1,1,1,1,2,2,1,1,1,2,2,2,1,2,2,1,1,1,2,2,1,1,2,1,1,1,1,1,1,1,1,2,1,2,1,2,1,2,2,2,2,1,1,2,3,2,1,2,1,2,2,1,1,2,1,1,1,1,1,1,1,1,1,2,2,2,1,2,2,2,1,1,1,1,1,1,1,2,2,1,2,2,1,1,1,1,1,1},
        //              情報技術研究部(じょぎ) では、主にコンピュータを用いたデジタルコンテンツの創作活動を行っています。ゲームやソフトウェア、 w e b アプリの開発、部員同士での情報共有などを通してプログラミングや 2 D・ 3 Dグラフィックなどの知識や技術を身に着けることを目的として活動を行っています。制作した作品は部内での作品発表会や立花祭といったイベントで展示を行い、他の人からの感想や改善点を次の作品に活かすことで技術向上に努めています。前期の間に、新入生向けのプログラミングや w e bアプリ制作などの基礎を学ぶ講座を部内で毎年行っています。知識や技術がなくても参加できる講座になっているので、興味のある方は是非参加してみてください。
        new List<int>(){3,2,1,3,2,3,1,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,1,2,2,1,1,1,3,2,3,2,1,1,1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,1,1,3,1,1,1,1,1,1,1,1,1,2,2,1,1,1,2,2,1,3,1,1,1,1,1,1,2,2,1,1,2,2,1,1,2,1,1,2,2,2,3,2,1,2,2,2,1,1,1,1,1,1,1,1,1,2,1,1,3,1,1,2,1,2,1,1,1,2,2,1,2,2,2,1,2,1,2,2,1,1,1,1,1,1,1,1,3,2,3,1,2,1,1,1,1,1,1,2,1,1,3,1,1,2,3,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,1,1,1,1,1,1,2,1,2,1,1,1,2,1,2,2,3,1,1,1,1,1,1,1,2,1,1,3,1,1,1,1,1,2,1,1,1,1,2,1,1,1,1,1,1,1,1,1,1,3,1,1,1,1,2,1,1,1,2,1,1,1,1,1,1,1,1,1,1},
        //              本学大学院は、工学部、情報工学部の上に工学研究科、社会環境学部の上に社会環境学研究科を設置し、活発な研究活動を行っています。修士課程を修了すると、修士（工学または社会環境学）の学位が、博士後期課程の最終試験に合格すると博士（工学）の学位が、それぞれ授与されます。
        new List<int>(){2,2,2,2,2,1,1,2,2,1,1,3,2,2,2,1,1,2,1,2,2,2,3,1,1,2,2,2,3,2,1,1,2,1,2,2,2,3,2,2,3,1,1,2,1,1,1,2,2,1,2,3,2,2,1,3,1,1,1,1,1,1,3,1,1,2,1,3,3,1,1,1,1,3,1,1,2,2,1,1,1,2,2,2,3,2,1,1,2,1,1,1,2,1,2,1,1,2,1,2,3,1,2,1,2,2,1,1,1,2,1,1,2,2,1,1,2,1,1,1,1,1,1,1,2,1,1,1,1,1,1},
        //              工学分野の基盤となる知識と技術ならびにグローバルな視点を有し、かつそれらを社会の安全・安心な発展のために用いる倫理観と問題解決能力、主体性を備えた実践型人材の育成を目的とする。
        new List<int>(){2,2,2,1,1,1,2,1,1,1,1,2,1,1,3,1,1,1,1,1,1,1,1,1,1,1,2,1,2,1,1,1,1,1,1,1,1,2,2,1,2,2,1,2,2,1,2,2,1,1,1,1,2,1,1,2,1,2,1,2,2,2,2,2,3,1,2,2,2,1,2,1,1,2,2,2,2,2,1,2,2,1,2,2,1,1,1,1},
        //              情報工学およびコンピュータ利用技術に関して、数理系の専門基礎から情報工学の幅広い応用に関する専門分野までを教授研究し、グローバル化・高度情報化が進展する社会において、修得した知識や技術を活用し、主体的に課題解決ができる実践型人材の育成を目的とする。
        new List<int>(){3,2,2,2,1,1,1,1,1,1,1,1,1,1,2,1,3,1,2,1,1,1,2,1,2,1,2,2,1,1,1,1,3,2,2,2,1,2,2,1,2,2,1,2,1,1,2,2,2,1,1,1,1,3,2,2,3,1,1,1,1,1,1,1,1,1,2,1,3,2,1,1,2,2,1,1,2,2,1,1,1,1,1,3,2,1,1,1,2,1,1,3,1,2,2,1,1,2,2,2,1,1,2,2,2,1,1,1,1,2,2,2,2,2,1,2,2,1,2,2,1,1,1,1},
        //              環境にかかわる諸問題に関して主として社会科学の立場からアプローチし、社会の仕組みを理解した上で、グローバルな視点から持続可能な社会実現に主体的・自律的に貢献することのできる実践型人材の育成を目的とする。
        new List<int>(){2,3,1,1,1,1,1,2,2,2,1,2,1,1,2,1,1,1,2,2,1,2,1,2,1,1,1,1,1,1,1,1,1,1,2,2,1,1,1,1,1,1,2,1,1,2,1,1,1,1,1,1,1,1,1,2,1,1,1,2,1,2,1,2,2,2,2,1,2,2,2,1,1,2,2,1,2,2,1,1,1,1,1,1,1,1,2,2,2,2,2,1,2,2,1,2,2,1,1,1,1},
    
    
    };


 
}
