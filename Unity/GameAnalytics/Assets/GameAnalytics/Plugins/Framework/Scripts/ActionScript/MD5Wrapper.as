package

{

    import com.meychi.ascrypt3.MD5;

    public class MD5Wrapper

    {

        public static function Md5Sum(source:String):String

        {

            return MD5.calculate(source);

        }

    }

}