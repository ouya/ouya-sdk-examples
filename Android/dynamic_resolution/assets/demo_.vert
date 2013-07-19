//----------------------------------------------------------------------------------
// File:        android/dynamic_resolution/assets/demo_.vert
// SDK Version: v10.00 
// Email:       tegradev@nvidia.com
// Site:        http://developer.nvidia.com/
//
// Copyright (c) 2007-2012, NVIDIA CORPORATION.  All rights reserved.
//
// TO  THE MAXIMUM  EXTENT PERMITTED  BY APPLICABLE  LAW, THIS SOFTWARE  IS PROVIDED
// *AS IS*  AND NVIDIA AND  ITS SUPPLIERS DISCLAIM  ALL WARRANTIES,  EITHER  EXPRESS
// OR IMPLIED, INCLUDING, BUT NOT LIMITED  TO, IMPLIED WARRANTIES OF MERCHANTABILITY
// AND FITNESS FOR A PARTICULAR PURPOSE.  IN NO EVENT SHALL  NVIDIA OR ITS SUPPLIERS
// BE  LIABLE  FOR  ANY  SPECIAL,  INCIDENTAL,  INDIRECT,  OR  CONSEQUENTIAL DAMAGES
// WHATSOEVER (INCLUDING, WITHOUT LIMITATION,  DAMAGES FOR LOSS OF BUSINESS PROFITS,
// BUSINESS INTERRUPTION, LOSS OF BUSINESS INFORMATION, OR ANY OTHER PECUNIARY LOSS)
// ARISING OUT OF THE  USE OF OR INABILITY  TO USE THIS SOFTWARE, EVEN IF NVIDIA HAS
// BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
//
//
//----------------------------------------------------------------------------------

uniform float aspect;

attribute vec4  pos_attr;
attribute float rot0_attr;
attribute float rot1_attr;
attribute vec2  rot_center_attr;
attribute vec4  col_attr;

uniform float time;

varying vec4 col;
varying vec2 pos;
varying vec2 center;

uniform vec2 s_vert[3];
 
    //gl_Position.x =         cos * pos_attr.x - sin * pos_attr.y + translation.x;
    //gl_Position.y = aspect*(sin * pos_attr.x + cos * pos_attr.y + translation.y);

void main()
{
    float ang0 = time * rot0_attr;
    float ang1 = time * rot1_attr;

    vec2  offset= pos_attr.xy;
    float scale = pos_attr.w;
    int   idx   = int(pos_attr.z);

	pos = s_vert[idx];

    // apply scale
    pos.xy = pos.xy * scale;

    vec2 newPos;

    // rotate about center of tri
#if 0
    {
        float s = sin(ang0);
        float c = cos(ang0);

        newPos.x = pos.x * c - pos.y * s;
        newPos.y = pos.x * s + pos.y * c;
    }
#else
    newPos = pos;
#endif
    pos = newPos;
    

    // rotate about some screen point
#if 0
    {
        float c = cos(ang1);
        float s = sin(ang1);
        offset -= rot_center_attr;
        newPos.x = offset.x * c - offset.y * s;
        newPos.y = offset.x * s + offset.y * c;
        newPos += rot_center_attr;
    }
#else
    newPos = offset;
#endif

    // offset
    pos += newPos;

    // correct for aspect ratio
    pos.y = pos.y * aspect;

    gl_Position.xy = pos.xy;
    gl_Position.z = 0.0;
    gl_Position.w = 1.0;

	center = rot_center_attr;
    col = col_attr;
}


