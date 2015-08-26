package com.epicgames.ue4;

import tv.ouya.examples.unreal.spritetest.OBBDownloaderService;
import tv.ouya.examples.unreal.spritetest.DownloaderActivity;


public class DownloadShim
{
	public static OBBDownloaderService DownloaderService;
	public static DownloaderActivity DownloadActivity;
	public static Class<DownloaderActivity> GetDownloaderType() { return DownloaderActivity.class; }
}

