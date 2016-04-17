// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using Ara2.StreamMD5;

namespace Ara2
{
    static class AraToolsInternalStatic
    {
        public static Dictionary<string, string> ContextTypeDic = new Dictionary<string, string>{
			{"ez","application/andrew-inset"},
			{"hqx","application/mac-binhex40"},
			{"cpt","application/mac-compactpro"},
			{"mathml","application/mathml+xml"},
			{"doc","application/msword"},
			{"bin","application/octet-stream"},
			{"dms","application/octet-stream"},
			{"lha","application/octet-stream"},
			{"lzh","application/octet-stream"},
			{"exe","application/octet-stream"},
			{"class","application/octet-stream"},
			{"so","application/octet-stream"},
			{"dll","application/octet-stream"},
			{"oda","application/oda"},
			{"ogg","application/ogg"},
			{"pdf","application/pdf"},
			{"ai","application/postscript"},
			{"eps","application/postscript"},
			{"ps","application/postscript"},
			{"rdf","application/rdf+xml"},
			{"smi","application/smil"},
			{"smil","application/smil"},
			{"gram","application/srgs"},
			{"grxml","application/srgs+xml"},
			{"mif","application/vnd.mif"},
			{"xul","application/vnd.mozilla.xul+xml"},
			{"xls","application/vnd.ms-excel"},
			{"ppt","application/vnd.ms-powerpoint"},
			{"wbxml","application/vnd.wap.wbxml"},
			{"wmlc","application/vnd.wap.wmlc"},
			{"wmlsc","application/vnd.wap.wmlscriptc"},
			{"vxml","application/voicexml+xml"},
			{"bcpio","application/x-bcpio"},
			{"vcd","application/x-cdlink"},
			{"pgn","application/x-chess-pgn"},
			{"cpio","application/x-cpio"},
			{"csh","application/x-csh"},
			{"dcr","application/x-director"},
			{"dir","application/x-director"},
			{"dxr","application/x-director"},
			{"dvi","application/x-dvi"},
			{"spl","application/x-futuresplash"},
			{"gtar","application/x-gtar"},
			{"hdf","application/x-hdf"},
			{"php","application/x-httpd-php"},
			{"php4","application/x-httpd-php"},
			{"php3","application/x-httpd-php"},
			{"phtml","application/x-httpd-php"},
			{"phps","application/x-httpd-php-source"},
			{"js","application/x-javascript"},
			{"skp","application/x-koan"},
			{"skd","application/x-koan"},
			{"skt","application/x-koan"},
			{"skm","application/x-koan"},
			{"latex","application/x-latex"},
			{"nc","application/x-netcdf"},
			{"cdf","application/x-netcdf"},
			{".crl","application/x-pkcs7-crl"},
			{"sh","application/x-sh"},
			{"shar","application/x-shar"},
			{"swf","application/x-shockwave-flash"},
			{"sit","application/x-stuffit"},
			{"sv4cpio","application/x-sv4cpio"},
			{"sv4crc","application/x-sv4crc"},
			{"tgz","application/x-tar"},
			{"tar","application/x-tar"},
			{"tcl","application/x-tcl"},
			{"tex","application/x-tex"},
			{"texinfo","application/x-texinfo"},
			{"texi","application/x-texinfo"},
			{"t","application/x-troff"},
			{"tr","application/x-troff"},
			{"roff","application/x-troff"},
			{"man","application/x-troff-man"},
			{"me","application/x-troff-me"},
			{"ms","application/x-troff-ms"},
			{"ustar","application/x-ustar"},
			{"src","application/x-wais-source"},
			{"crt","application/x-x509-ca-cert"},
			{"xhtml","application/xhtml+xml"},
			{"xht","application/xhtml+xml"},
			{"xml","application/xml"},
			{"xsl","application/xml"},
			{"dtd","application/xml-dtd"},
			{"xslt","application/xslt+xml"},
			{"zip","application/zip"},
			{"au","audio/basic"},
			{"snd","audio/basic"},
			{"mid","audio/midi"},
			{"midi","audio/midi"},
			{"kar","audio/midi"},
			{"mpga","audio/mpeg"},
			{"mp2","audio/mpeg"},
			{"mp3","audio/mpeg"},
			{"aif","audio/x-aiff"},
			{"aiff","audio/x-aiff"},
			{"aifc","audio/x-aiff"},
			{"m3u","audio/x-mpegurl"},
			{"ram","audio/x-pn-realaudio"},
			{"rm","audio/x-pn-realaudio"},
			{"rpm","audio/x-pn-realaudio-plugin"},
			{"ra","audio/x-realaudio"},
			{"wav","audio/x-wav"},
			{"pdb","chemical/x-pdb"},
			{"xyz","chemical/x-xyz"},
			{"bmp","image/bmp"},
			{"cgm","image/cgm"},
			{"gif","image/gif"},
			{"ief","image/ief"},
			{"jpeg","image/jpeg"},
			{"jpg","image/jpeg"},
			{"jpe","image/jpeg"},
			{"png","image/png"},
			{"svg","image/svg+xml"},
			{"tiff","image/tiff"},
			{"tif","image/tiff"},
			{"djvu","image/vnd.djvu"},
			{"djv","image/vnd.djvu"},
			{"wbmp","image/vnd.wap.wbmp"},
			{"ras","image/x-cmu-raster"},
			{"ico","image/x-icon"},
			{"pnm","image/x-portable-anymap"},
			{"pbm","image/x-portable-bitmap"},
			{"pgm","image/x-portable-graymap"},
			{"ppm","image/x-portable-pixmap"},
			{"rgb","image/x-rgb"},
			{"xbm","image/x-xbitmap"},
			{"xpm","image/x-xpixmap"},
			{"xwd","image/x-xwindowdump"},
			{"igs","model/iges"},
			{"iges","model/iges"},
			{"msh","model/mesh"},
			{"mesh","model/mesh"},
			{"silo","model/mesh"},
			{"wrl","model/vrml"},
			{"vrml","model/vrml"},
			{"ics","text/calendar"},
			{"ifb","text/calendar"},
			{"css","text/css"},
			{".shtml","text/html"},
			{"html","text/html"},
			{"htm","text/html"},
			{"asc","text/plain"},
			{"txt","text/plain"},
			{"rtx","text/richtext"},
			{"rtf","text/rtf"},
			{"sgml","text/sgml"},
			{"sgm","text/sgml"},
			{"tsv","text/tab-separated-values"},
			{"wml","text/vnd.wap.wml"},
			{"wmls","text/vnd.wap.wmlscript"},
			{"etx","text/x-setext"},
			{"mpeg","video/mpeg"},
			{"mpg","video/mpeg"},
			{"mpe","video/mpeg"},
			{"qt","video/quicktime"},
			{"mov","video/quicktime"},
			{"mxu","video/vnd.mpegurl"},
			{"avi","video/x-msvideo"},
			{"movie","video/x-sgi-movie"},
			{"ice","x-conference/x-cooltalk"}
        };

        static Hashtable CacheFilesInternal;
        static Hashtable CacheFilesInternal_ETag;

        static public string GetMD5FileInternal(string vFile,Stream stream)
        {
            string vMD5 = "";

            try
            {
                vMD5 = (string)CacheFilesInternal[vFile];
            }
            catch
            {
                CacheFilesInternal = new Hashtable();
                CacheFilesInternal_ETag = new Hashtable();
            }

            if (vMD5 == "" || vMD5 == null)
            {
                ClassStreamMD5 StreamMD5 = new ClassStreamMD5();
                vMD5 = StreamMD5.CalculateMD5(stream);
                CacheFilesInternal[vFile] = vMD5;
                CacheFilesInternal_ETag[vMD5] = vFile;
            }

            return vMD5;
        }

        static public string GetFileInternalByMD5(string vMD5)
        {
            string vFile = null;
            try
            {
                vFile = (string)CacheFilesInternal_ETag[vMD5];
            }
            catch { }
            return vFile;
        }


        

        
        
    }

}
