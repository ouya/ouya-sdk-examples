#include "AudioFormat.h"

namespace audio_media_AudioFormat
{
	int AudioFormat::CHANNEL_OUT_MONO()
	{
		return 4;
	}

	int AudioFormat::CHANNEL_OUT_STEREO()
	{
		return 12;
	}

	int AudioFormat::ENCODING_PCM_16BIT()
	{
		return 2;
	}

	int AudioFormat::ENCODING_PCM_8BIT()
	{
		return 3;
	}
}