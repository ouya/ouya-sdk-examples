/**
* MD5 Encryption.
* modified by Jeff Houser, DotComIt 11/1/06
* http://www.jeffryhouser.com
* Converted from ActionSCript 2.0 code to ActionScript 3.0 code for use in a Flex 2 Application
* @Version 3.0
* Released under the same license as the previous code-bases
* That means there is no license, use it as you see fit for whatever reason you want
* 
* Calculates the MD5 checksum.
* @authors Mika Palmu
* @version 2.0
*
* Original Javascript implementation:
* RSA Data Security, Inc. MD5 Message Digest Algorithm, as defined in RFC 1321.
* Version 2.1 Copyright Paul Johnston 1999 - 2002
* Other contributors: Greg Holt, Andrew Kepert, Ydnar, Lostinet
* See http://pajhome.org.uk/crypt/md5 for more info.
*/

// JH DotComIt 11/1/06 added package definition 
package com.meychi.ascrypt3{

	public class MD5 {

		/**
		* Calculates the MD5 checksum.
		*/
		// JH DotComIt 11/1/06 remove static 
		public static function calculate(src:String):String {
            return hex_md5(src);
        }
	
		/**
		* Private methods.
		*/
		// JH DotComIt 11/1/06 remove static 
		private static function hex_md5(src:String):String {
			return binl2hex(core_md5(str2binl(src), src.length*8));
		}

		// JH DotComIt 11/1/06 remove static 
		private static function core_md5(x:Array, len:Number):Array {
			x[len >> 5] |= 0x80 << ((len)%32);
			x[(((len+64) >>> 9) << 4)+14] = len;
			var a:Number = 1732584193, b:Number = -271733879;
			var c:Number = -1732584194, d:Number = 271733878;
			for (var i:Number = 0; i<x.length; i += 16) {
				var olda:Number = a, oldb:Number = b;
				var oldc:Number = c, oldd:Number = d;
				a = md5_ff(a, b, c, d, x[i+0], 7, -680876936);
				d = md5_ff(d, a, b, c, x[i+1], 12, -389564586);
				c = md5_ff(c, d, a, b, x[i+2], 17, 606105819);
				b = md5_ff(b, c, d, a, x[i+3], 22, -1044525330);
				a = md5_ff(a, b, c, d, x[i+4], 7, -176418897);
				d = md5_ff(d, a, b, c, x[i+5], 12, 1200080426);
				c = md5_ff(c, d, a, b, x[i+6], 17, -1473231341);
				b = md5_ff(b, c, d, a, x[i+7], 22, -45705983);
				a = md5_ff(a, b, c, d, x[i+8], 7, 1770035416);
				d = md5_ff(d, a, b, c, x[i+9], 12, -1958414417);
				c = md5_ff(c, d, a, b, x[i+10], 17, -42063);
				b = md5_ff(b, c, d, a, x[i+11], 22, -1990404162);
				a = md5_ff(a, b, c, d, x[i+12], 7, 1804603682);
				d = md5_ff(d, a, b, c, x[i+13], 12, -40341101);
				c = md5_ff(c, d, a, b, x[i+14], 17, -1502002290);
				b = md5_ff(b, c, d, a, x[i+15], 22, 1236535329);
				a = md5_gg(a, b, c, d, x[i+1], 5, -165796510);
				d = md5_gg(d, a, b, c, x[i+6], 9, -1069501632);
				c = md5_gg(c, d, a, b, x[i+11], 14, 643717713);
				b = md5_gg(b, c, d, a, x[i+0], 20, -373897302);
				a = md5_gg(a, b, c, d, x[i+5], 5, -701558691);
				d = md5_gg(d, a, b, c, x[i+10], 9, 38016083);
				c = md5_gg(c, d, a, b, x[i+15], 14, -660478335);
				b = md5_gg(b, c, d, a, x[i+4], 20, -405537848);
				a = md5_gg(a, b, c, d, x[i+9], 5, 568446438);
				d = md5_gg(d, a, b, c, x[i+14], 9, -1019803690);
				c = md5_gg(c, d, a, b, x[i+3], 14, -187363961);
				b = md5_gg(b, c, d, a, x[i+8], 20, 1163531501);
				a = md5_gg(a, b, c, d, x[i+13], 5, -1444681467);
				d = md5_gg(d, a, b, c, x[i+2], 9, -51403784);
				c = md5_gg(c, d, a, b, x[i+7], 14, 1735328473);
				b = md5_gg(b, c, d, a, x[i+12], 20, -1926607734);
				a = md5_hh(a, b, c, d, x[i+5], 4, -378558);
				d = md5_hh(d, a, b, c, x[i+8], 11, -2022574463);
				c = md5_hh(c, d, a, b, x[i+11], 16, 1839030562);
				b = md5_hh(b, c, d, a, x[i+14], 23, -35309556);
				a = md5_hh(a, b, c, d, x[i+1], 4, -1530992060);
				d = md5_hh(d, a, b, c, x[i+4], 11, 1272893353);
				c = md5_hh(c, d, a, b, x[i+7], 16, -155497632);
				b = md5_hh(b, c, d, a, x[i+10], 23, -1094730640);
				a = md5_hh(a, b, c, d, x[i+13], 4, 681279174);
				d = md5_hh(d, a, b, c, x[i+0], 11, -358537222);
				c = md5_hh(c, d, a, b, x[i+3], 16, -722521979);
				b = md5_hh(b, c, d, a, x[i+6], 23, 76029189);
				a = md5_hh(a, b, c, d, x[i+9], 4, -640364487);
				d = md5_hh(d, a, b, c, x[i+12], 11, -421815835);
				c = md5_hh(c, d, a, b, x[i+15], 16, 530742520);
				b = md5_hh(b, c, d, a, x[i+2], 23, -995338651);
				a = md5_ii(a, b, c, d, x[i+0], 6, -198630844);
				d = md5_ii(d, a, b, c, x[i+7], 10, 1126891415);
				c = md5_ii(c, d, a, b, x[i+14], 15, -1416354905);
				b = md5_ii(b, c, d, a, x[i+5], 21, -57434055);
				a = md5_ii(a, b, c, d, x[i+12], 6, 1700485571);
				d = md5_ii(d, a, b, c, x[i+3], 10, -1894986606);
				c = md5_ii(c, d, a, b, x[i+10], 15, -1051523);
				b = md5_ii(b, c, d, a, x[i+1], 21, -2054922799);
				a = md5_ii(a, b, c, d, x[i+8], 6, 1873313359);
				d = md5_ii(d, a, b, c, x[i+15], 10, -30611744);
				c = md5_ii(c, d, a, b, x[i+6], 15, -1560198380);
				b = md5_ii(b, c, d, a, x[i+13], 21, 1309151649);
				a = md5_ii(a, b, c, d, x[i+4], 6, -145523070);
				d = md5_ii(d, a, b, c, x[i+11], 10, -1120210379);
				c = md5_ii(c, d, a, b, x[i+2], 15, 718787259);
				b = md5_ii(b, c, d, a, x[i+9], 21, -343485551);
				a = safe_add(a, olda); b = safe_add(b, oldb);
				c = safe_add(c, oldc); d = safe_add(d, oldd);
			}
			return new Array(a, b, c, d);
		}


		private static function md5_cmn(q:Number, a:Number, b:Number, x:Number, s:Number, t:Number):Number {
			return safe_add(bit_rol(safe_add(safe_add(a, q), safe_add(x, t)), s), b);
		}
		private static function md5_ff(a:Number, b:Number, c:Number, d:Number, x:Number, s:Number, t:Number):Number {
			return md5_cmn((b & c) | ((~b) & d), a, b, x, s, t);
		}
		private static function md5_gg(a:Number, b:Number, c:Number, d:Number, x:Number, s:Number, t:Number):Number {
			return md5_cmn((b & d) | (c & (~d)), a, b, x, s, t);
		}
		private static function md5_hh(a:Number, b:Number, c:Number, d:Number, x:Number, s:Number, t:Number):Number {
			return md5_cmn(b ^ c ^ d, a, b, x, s, t);
		}
		private static function md5_ii(a:Number, b:Number, c:Number, d:Number, x:Number, s:Number, t:Number):Number {
			return md5_cmn(c ^ (b | (~d)), a, b, x, s, t);
		}
		private static function bit_rol(num:Number, cnt:Number):Number {
			return (num << cnt) | (num >>> (32-cnt));
		}
		private static function safe_add(x:Number, y:Number):Number {
			var lsw:Number = (x & 0xFFFF)+(y & 0xFFFF);
			var msw:Number = (x >> 16)+(y >> 16)+(lsw >> 16);
			return (msw << 16) | (lsw & 0xFFFF);
		}
		private static function str2binl(str:String):Array {
			var bin:Array = new Array();
			var mask:Number = (1 << 8)-1;
			for (var i:Number = 0; i<str.length*8; i += 8) {
				bin[i >> 5] |= (str.charCodeAt(i/8) & mask) << (i%32);
			}
			return bin;
		}
		private static function binl2hex(binarray:Array):String {
			var str:String = new String("");
			var tab:String = new String("0123456789abcdef");
			for (var i:Number = 0; i<binarray.length*4; i++) {
				str += tab.charAt((binarray[i >> 2] >> ((i%4)*8+4)) & 0xF) + tab.charAt((binarray[i >> 2] >> ((i%4)*8)) & 0xF);
			}
			return str;
		}
	}
}