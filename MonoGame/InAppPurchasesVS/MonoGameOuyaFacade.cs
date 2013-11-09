using System.Collections.Generic;

namespace Tv.Ouya.Sdk.Android
{
    public class MonoGameOuyaFacade
    {
        private static MonoGameOuyaFacade m_instance = null;

        public static MonoGameOuyaFacade Instance
        {
            get
            {
                if (null == m_instance)
                {
                    m_instance = new MonoGameOuyaFacade();
                }
                return m_instance;
            }
        }

        private MonoGameOuyaFacade()
        {
        }
    }
}
