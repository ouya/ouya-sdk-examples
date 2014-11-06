#if UNITY_ANDROID && !UNITY_EDITOR
using Android.Graphics;
using java.io;
using Java.IO.InputStream;
using Java.IO.OutputStream;
using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using tv.ouya.console.api;
using tv.ouya.console.api.content;
using tv.ouya.sdk;
#endif
using UnityEngine;

public class OuyaCommunityContent : MonoBehaviour
#if UNITY_ANDROID && !UNITY_EDITOR
    ,
    OuyaSDK.IContentInitializedListener,
    OuyaSDK.IContentDeleteListener,
    OuyaSDK.IContentDownloadListener,
    OuyaSDK.IContentInstalledSearchListener,
    OuyaSDK.IContentPublishedSearchListener,
    OuyaSDK.IContentPublishListener,
    OuyaSDK.IContentSaveListener,
    OuyaSDK.IContentUnpublishListener,
    OuyaSDK.IResumeListener
#endif
{
    public Texture2D[] m_textureScreenshots = null;

#if UNITY_ANDROID && !UNITY_EDITOR
    
    /// <summary>
    /// Handle focusing items
    /// </summary>
    private FocusManager m_focusManager = new FocusManager();

    /// <summary>
    /// Buttons
    /// </summary>
    private object m_btnSearchByRating = new object();
    private object m_btnSearchByCreatedAt = new object();
    private object m_btnSearchByUpdatedAt = new object();
    private object m_btnCreate = new object();
    private bool m_buttonClicked = false;

    /// <summary>
    /// Visual status indicator
    /// </summary>
    private string m_status = "Status:";

    /// <summary>
    /// Indicates community content is available
    /// </summary>
    private bool m_isAvailable = false;

    private string m_lblInstalled = "Installed Results: count=0 list count=0";
    private string m_lblPublished = "Published Results: count=0 list count=0";

    /// <summary>
    /// Reference to screenshot to save with community content
    /// </summary>
    private Bitmap[] m_bitmapScreenshots = null;

    /// <summary>
    /// The list of installed mods
    /// </summary>
    private List<WidgetOuyaMod> m_widgets = new List<WidgetOuyaMod>();

    /// <summary>
    /// The list of pending results for the update event
    /// </summary>
    private List<OuyaMod> m_pendingInstalledSearchResults = null;

    /// <summary>
    /// The list of pending results for the update event
    /// </summary>
    private List<OuyaMod> m_pendingPublishedSearchResults = null;

    /// <summary>
    /// The selected sort method
    /// </summary>
    private OuyaContent.SortMethod m_sortMethod = OuyaContent.SortMethod.Rating;

    private class WidgetOuyaMod
    {
        public OuyaMod m_instance = null;
        public bool m_searchByInstalled = false;
        public bool m_searchByPublished = false;
        public object m_buttonEdit = new object();
        public object m_buttonPublish = new object();
        public object m_buttonDelete = new object();
        public object m_buttonDownload = new object();
        public object m_buttonFlag = new object();
        public object m_buttonRate = new object();
        public string m_category = string.Empty;
        public string m_description = string.Empty;
        public string m_filenames = string.Empty;
        public string[] m_files = null;
        public string m_metaData = string.Empty;
        public Texture2D[] m_screenshots = null;
        public string m_tags = string.Empty;
        public Texture2D[] m_thumbnails = null;
        public string m_title = string.Empty;
        public bool m_isPublished = false;
        public bool m_isDownloading = false;
        public bool m_isInstalled = false;
        public bool m_isFlagged = false;
        public int m_ratingCount = 0;
        public float m_ratingAverage = 0;
        public float? m_userRating = null;
    }

    private void Awake()
    {
        OuyaSDK.registerContentInitializedListener(this);
        OuyaSDK.registerContentDeleteListener(this);
        OuyaSDK.registerContentDownloadListener(this);
        OuyaSDK.registerContentInstalledSearchListener(this);
        OuyaSDK.registerContentPublishedSearchListener(this);
        OuyaSDK.registerContentPublishListener(this);
        OuyaSDK.registerContentSaveListener(this);
        OuyaSDK.registerContentUnpublishListener(this);
        OuyaSDK.registerResumeListener(this);

        if (null != m_textureScreenshots)
        {
            m_bitmapScreenshots = new Bitmap[m_textureScreenshots.Length];
            for (int index = 0; index < m_textureScreenshots.Length; ++index)
            { 
                Texture2D texture = m_textureScreenshots[index];

                Debug.Log("Encoding PNG from texture bytes");

                // make texture readable
                byte[] buffer = texture.EncodeToPNG();
                if (null == buffer ||
                    buffer.Length == 0)
                {
                    Debug.LogError("Failed to encode png");
                }
                else
                {
                    Debug.Log("Converting bytes to bitmap");
                    m_bitmapScreenshots[index] = BitmapFactory.decodeByteArray(buffer, 0, buffer.Length);
                }
            }
        }

        ResetFocus();
    }
    private void OnDestroy()
    {
        OuyaSDK.unregisterContentInitializedListener(this);
        OuyaSDK.unregisterContentDeleteListener(this);
        OuyaSDK.unregisterContentDownloadListener(this);
        OuyaSDK.unregisterContentInstalledSearchListener(this);
        OuyaSDK.unregisterContentPublishedSearchListener(this);
        OuyaSDK.unregisterContentPublishListener(this);
        OuyaSDK.unregisterContentSaveListener(this);
        OuyaSDK.unregisterContentUnpublishListener(this);
        OuyaSDK.unregisterResumeListener(this);
    }

    /// <summary>
    /// IContentInitializedListener
    /// </summary>
    public void ContentInitializedOnInitialized()
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        using (OuyaContent ouyaContent = OuyaUnityPlugin.getOuyaContent())
        {
            if (null == ouyaContent)
            {
                Debug.LogError("OuyaContent is null");
                return;
            }
            m_isAvailable = ouyaContent.isAvailable();
        }

        RunSearch();
    }
    public void ContentInitializedOnDestroyed()
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentDeleteListener
    /// </summary>
    public void ContentDeleteOnDeleted(OuyaMod ouyaMod)
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        RunSearch();
    }
    public void ContentDeleteOnDeleteFailed(OuyaMod ouyaMod, int code, string reason)
    {
        m_status = string.Format("Status: {0} code={1} reason={2}", MethodBase.GetCurrentMethod().Name, code, reason);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentDownloadListener
    /// </summary>
    public void ContentDownloadOnComplete(OuyaMod ouyaMod)
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        RunSearch();
    }
    public void ContentDownloadOnFailed(OuyaMod ouyaMod)
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);
    }
    public void ContentDownloadOnProgress(OuyaMod ouyaMod, int progress)
    {
        m_status = string.Format("Status: {0} progress={1}", MethodBase.GetCurrentMethod().Name, progress);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentInstalledSearchListener
    /// </summary>
    public void ContentInstalledSearchOnResults(List<OuyaMod> ouyaMods, int count)
    {
        m_status = string.Format("Status: {0} count={1}", MethodBase.GetCurrentMethod().Name, count);
        Debug.Log(m_status);

        if (null == ouyaMods)
        {
            m_lblInstalled = string.Format("Installed Results: null list count={0}", count);
        }
        else
        {
            m_lblInstalled = string.Format("Installed Results: count={0} list count={1}", ouyaMods.Count, count);
            m_pendingInstalledSearchResults = ouyaMods;
        }
    }
    public void ContentInstalledSearchOnError(int code, string reason)
    {
        m_status = string.Format("Status: {0} code={1} reason={2}", MethodBase.GetCurrentMethod().Name, code, reason);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentPublishedSearchListener
    /// </summary>
    public void ContentPublishedSearchOnResults(List<OuyaMod> ouyaMods, int count)
    {
        m_status = string.Format("Status: {0} count={1}", MethodBase.GetCurrentMethod().Name, count);
        Debug.Log(m_status);

        if (null == ouyaMods)
        {
            m_lblPublished = string.Format("Published Results: null list count={0}", count);
        }
        else
        {
            m_lblPublished = string.Format("Published Results: count={0} list count={1}", ouyaMods.Count, count);
            m_pendingPublishedSearchResults = ouyaMods;
        }
    }
    public void ContentPublishedSearchOnError(int code, string reason)
    {
        m_status = string.Format("Status: {0} code={1} reason={2}", MethodBase.GetCurrentMethod().Name, code, reason);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentPublishListener
    /// </summary>
    public void ContentPublishOnSuccess(OuyaMod ouyaMod)
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        RunSearch();
    }
    public void ContentPublishOnError(OuyaMod ouyaMod, int code, string reason)
    {
        m_status = string.Format("Status: {0} code={1} reason={2}", MethodBase.GetCurrentMethod().Name, code, reason);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentSaveListener
    /// </summary>
    public void ContentSaveOnSuccess(OuyaMod ouyaMod)
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        RunSearch();
    }
    public void ContentSaveOnError(OuyaMod ouyaMod, int code, string reason)
    {
        m_status = string.Format("Status: {0} code={1} reason={2}", MethodBase.GetCurrentMethod().Name, code, reason);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IContentUnpublishListener
    /// </summary>
    public void ContentUnpublishOnSuccess(OuyaMod ouyaMod)
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        RunSearch();
    }
    public void ContentUnpublishOnError(OuyaMod ouyaMod, int code, string reason)
    {
        m_status = string.Format("Status: {0} code={1} reason={2}", MethodBase.GetCurrentMethod().Name, code, reason);
        Debug.Log(m_status);
    }

    /// <summary>
    /// IResumeListener
    /// </summary>
    public void OuyaOnResume()
    {
        m_status = string.Format("Status: {0}", MethodBase.GetCurrentMethod().Name);
        Debug.Log(m_status);

        if (m_isAvailable)
        {
            RunSearch();
        }
    }

    void OnGUI()
    {
        Color oldColor = GUI.backgroundColor;

        GUILayout.BeginVertical(GUILayout.Height(Screen.height));
        GUILayout.FlexibleSpace();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Unity Community Content Example");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label(m_isAvailable ? "Community Content is available" : "Community Content is not available");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label(m_status);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        if (m_focusManager.SelectedButton == m_btnSearchByRating)
        {
            GUI.backgroundColor = Color.red;
        }
        if (GUILayout.Button("Search by rating", GUILayout.Height(40)) ||
            (m_focusManager.SelectedButton == m_btnSearchByRating))
        {
            if (GetButtonUp(OuyaController.BUTTON_O))
            {
                m_buttonClicked = true;
            }
            else if (m_buttonClicked)
            {
                m_buttonClicked = false;
                m_sortMethod = OuyaContent.SortMethod.Rating;
                RunSearch();
            }
        }
        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        if (m_focusManager.SelectedButton == m_btnSearchByCreatedAt)
        {
            GUI.backgroundColor = Color.red;
        }
        if (GUILayout.Button("Search by created at", GUILayout.Height(40)) ||
            (m_focusManager.SelectedButton == m_btnSearchByCreatedAt))
        {
            if (GetButtonUp(OuyaController.BUTTON_O))
            {
                m_buttonClicked = true;
            }
            else if (m_buttonClicked)
            {
                m_buttonClicked = false;
                m_sortMethod = OuyaContent.SortMethod.CreatedAt;
                RunSearch();
            }
        }
        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        if (m_focusManager.SelectedButton == m_btnSearchByUpdatedAt)
        {
            GUI.backgroundColor = Color.red;
        }
        if (GUILayout.Button("Search by updated at", GUILayout.Height(40)) ||
            (m_focusManager.SelectedButton == m_btnSearchByUpdatedAt))
        {
            if (GetButtonUp(OuyaController.BUTTON_O))
            {
                m_buttonClicked = true;
            }
            else if (m_buttonClicked)
            {
                m_buttonClicked = false;
                m_sortMethod = OuyaContent.SortMethod.UpdatedAt;
                RunSearch();
            }
        }
        GUI.backgroundColor = oldColor;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUI.enabled = m_widgets.Count < 7;
        if (m_focusManager.SelectedButton == m_btnCreate)
        {
            GUI.backgroundColor = Color.red;
        }
        if ((GUILayout.Button("Create CC Item", GUILayout.Height(40)) ||
            (m_focusManager.SelectedButton == m_btnCreate)) &&
            GUI.enabled)
        {
            if (GetButtonUp(OuyaController.BUTTON_O))
            {
                m_buttonClicked = true;
            }
            else if (m_buttonClicked)
            {
                m_buttonClicked = false;
                Create();
            }
        }
        GUI.backgroundColor = oldColor;
        GUI.enabled = true;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label(m_lblInstalled);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        StringBuilder sb = new StringBuilder();

        foreach (WidgetOuyaMod ouyaMod in m_widgets)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
            
            GUILayout.Space(150);

            if (m_focusManager.SelectedButton == ouyaMod.m_buttonPublish)
            {
                GUI.backgroundColor = Color.red;
            }
            if (GUILayout.Button(ouyaMod.m_isPublished ? "Unpublish" : "Publish", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == ouyaMod.m_buttonPublish))
            {
                if (GetButtonUp(OuyaController.BUTTON_O))
                {
                    m_buttonClicked = true;
                }
                else if (m_buttonClicked)
                {
                    m_buttonClicked = false;
                    if (ouyaMod.m_isPublished)
                    {
                        Unpublish(ouyaMod.m_instance);
                    }
                    else
                    {
                        Publish(ouyaMod.m_instance);
                    }
                }
            }
            GUI.backgroundColor = oldColor;

            GUI.enabled = ouyaMod.m_searchByInstalled;
            if (m_focusManager.SelectedButton == ouyaMod.m_buttonDelete)
            {
                GUI.backgroundColor = Color.red;
            }
            if ((GUILayout.Button("Delete", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == ouyaMod.m_buttonDelete)) &&
                GUI.enabled)
            {
                if (GetButtonUp(OuyaController.BUTTON_O))
                {
                    m_buttonClicked = true;
                }
                else if (m_buttonClicked)
                {
                    m_buttonClicked = false;
                    Delete(ouyaMod.m_instance);
                }
            }
            GUI.backgroundColor = oldColor;
            GUI.enabled = true;

            GUI.enabled = ouyaMod.m_searchByPublished && !ouyaMod.m_isDownloading;
            if (m_focusManager.SelectedButton == ouyaMod.m_buttonDownload)
            {
                GUI.backgroundColor = Color.red;
            }
            if ((GUILayout.Button("Download", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == ouyaMod.m_buttonDownload)) &&
                GUI.enabled)
            {
                if (GetButtonUp(OuyaController.BUTTON_O))
                {
                    m_buttonClicked = true;
                }
                else if (m_buttonClicked)
                {
                    m_buttonClicked = false;
                    Download(ouyaMod.m_instance);
                }
            }
            GUI.backgroundColor = oldColor;
            GUI.enabled = true;

            GUI.enabled = ouyaMod.m_isPublished;
            if (m_focusManager.SelectedButton == ouyaMod.m_buttonRate)
            {
                GUI.backgroundColor = Color.red;
            }
            if ((GUILayout.Button("Rate", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == ouyaMod.m_buttonRate)) &&
                GUI.enabled)
            {
                if (GetButtonUp(OuyaController.BUTTON_O))
                {
                    m_buttonClicked = true;
                }
                else if (m_buttonClicked)
                {
                    m_buttonClicked = false;
                    Rate(ouyaMod.m_instance);
                }
            }
            GUI.backgroundColor = oldColor;
            GUI.enabled = true;

            GUI.enabled = ouyaMod.m_searchByInstalled;
            if (m_focusManager.SelectedButton == ouyaMod.m_buttonEdit)
            {
                GUI.backgroundColor = Color.red;
            }
            if ((GUILayout.Button("Edit", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == ouyaMod.m_buttonEdit)) &&
                GUI.enabled)
            {
                if (GetButtonUp(OuyaController.BUTTON_O))
                {
                    m_buttonClicked = true;
                }
                else if (m_buttonClicked)
                {
                    m_buttonClicked = false;
                    Edit(ouyaMod.m_instance);
                }
            }
            GUI.backgroundColor = oldColor;
            GUI.enabled = true;

            GUI.enabled = ouyaMod.m_isPublished;
            if (m_focusManager.SelectedButton == ouyaMod.m_buttonFlag)
            {
                GUI.backgroundColor = Color.red;
            }
            if ((GUILayout.Button("Flag", GUILayout.Height(40)) ||
                (m_focusManager.SelectedButton == ouyaMod.m_buttonFlag)) &&
                GUI.enabled)
            {
                if (GetButtonUp(OuyaController.BUTTON_O))
                {
                    m_buttonClicked = true;
                }
                else if (m_buttonClicked)
                {
                    m_buttonClicked = false;
                    Flag(ouyaMod.m_instance);
                }
            }
            GUI.backgroundColor = oldColor;
            GUI.enabled = true;

            if (sb.Length > 0)
            {
                sb.Remove(0, sb.Length);
            }

            if (ouyaMod.m_searchByInstalled)
            {
                sb.Append("[Installed]");
            }
            if (ouyaMod.m_searchByPublished)
            {
                sb.Append("[Published]");
            }

            sb.Append(" [Category]=");
            sb.Append(ouyaMod.m_category);
            sb.Append(" [Description]=");
            sb.Append(ouyaMod.m_description);
            sb.Append(" [Filenames]=");
            sb.Append(ouyaMod.m_filenames);
            if (null != ouyaMod.m_files)
            {
                foreach (String file in ouyaMod.m_files)
                {
                    sb.Append("***");
                    sb.Append(file);
                }
            }
            sb.Append(" [Title]=");
            sb.Append(ouyaMod.m_title);
            sb.Append(" [Tags]=");
            sb.Append(ouyaMod.m_tags);
            sb.Append(" [Metadata]=");
            sb.Append(null == ouyaMod.m_metaData ? string.Empty : ouyaMod.m_metaData);
            sb.Append(" [IsInstalled]=");
            sb.Append(ouyaMod.m_isInstalled.ToString());
            sb.Append(" [RatingCount]=");
            sb.Append(ouyaMod.m_ratingCount.ToString());
            sb.Append(" [RatingAverage]=");
            sb.Append(ouyaMod.m_ratingAverage.ToString());
            sb.Append(" [UserRating]=");
            sb.Append(null == ouyaMod.m_userRating ? "none" : ouyaMod.m_userRating.ToString());
            sb.Append(" [IsFlagged]=");
            sb.Append(ouyaMod.m_isFlagged.ToString());
            GUILayout.Label(sb.ToString());

            GUILayout.Label(" [Screenshots]=");
            if (null != ouyaMod.m_screenshots)
            {
                foreach (Texture2D texture in ouyaMod.m_screenshots)
                {
                    if (null != texture)
                    {
                        GUILayout.Label(texture, GUILayout.Width(128), GUILayout.Height(128));
                    }
                }
            }

            GUILayout.Label(" [Thumbnail]=");
            if (null != ouyaMod.m_thumbnails)
            {
                foreach (Texture2D texture in ouyaMod.m_thumbnails)
                {
                    if (null != texture)
                    {
                        GUILayout.Label(texture, GUILayout.Width(128), GUILayout.Height(128));
                    }
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.Space(150);
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label(m_lblPublished);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    public bool GetButtonUp(int button)
    {
        for (int index = 0; index < OuyaController.MAX_CONTROLLERS; ++index)
        {
            if (OuyaSDK.OuyaInput.GetButtonUp(index, button))
            {
                return true;
            }
        }
        return false;
    }

    public void Create()
    {
        using (OuyaContent ouyaContent = OuyaUnityPlugin.getOuyaContent())
        {
            if (null == ouyaContent)
            {
                Debug.LogError("OuyaContent is null");
                return;
            }
            if (ouyaContent.isInitialized())
            {
                //Debug.Log("OuyaContent is initialized");
                using (OuyaMod ouyaMod = ouyaContent.create())
                {
                    //Debug.Log("Created OuyaMod");
                    using (OuyaMod.Editor editor = ouyaMod.edit())
                    {
                        //Debug.Log("Access to Editor");

                        // Required fields:
                        editor.setTitle("Custom Level");
                        editor.setCategory("level");
                        editor.setDescription("This is my custom level");

                        using (OutputStream os = editor.newFile("level.dat"))
                        {
                            os.write(System.Text.UTF8Encoding.UTF8.GetBytes("Hello World"));
                            os.close();
                        }

                        if (m_bitmapScreenshots.Length > 0)
                        {
                            editor.addScreenshot(m_bitmapScreenshots[0]);
                        }

                        // Add optional tags:
                        editor.addTag("space");
                        editor.addTag("king of the hill");

                        // Add optional metadata for your own display:
                        editor.setMetadata("difficulty=4;theme=space;mode=koth");

                        OuyaUnityPlugin.saveOuyaMod(ouyaMod, editor);
                    }
                }
            }
            else
            {
                Debug.LogError("OuyaContent is not initialized");
            }
        }
    }

    public void Edit(OuyaMod ouyaMod)
    {
        using (OuyaMod.Editor editor = ouyaMod.edit())
        {
            //Debug.Log("Access to Editor");

            // Required fields:
            editor.setTitle("edit title");
            editor.setCategory("edit category");
            editor.setDescription("edit description");

            // replace file
            editor.deleteFile("level.dat");

            using (OutputStream os = editor.newFile("level.dat"))
            {
                os.write(System.Text.UTF8Encoding.UTF8.GetBytes("edit file"));
                os.close();
            }

            // remove old screenshot
            List<OuyaModScreenshot> screenshots = ouyaMod.getScreenshots();
            for (int index = 0; index < screenshots.Count; ++index)
            {
                using (OuyaModScreenshot ouyaModScreenshot = screenshots[index])
                {
                    editor.removeScreenshot(ouyaModScreenshot);
                }
            }

            // add new screenshot
            if (m_bitmapScreenshots.Length > 1)
            {
                Debug.Log("Adding screenshot");
                editor.addScreenshot(m_bitmapScreenshots[1]);
            }
            else
            {
                Debug.LogError("Missing screenshot");
            }

            // remove old tags
            foreach (string tag in ouyaMod.getTags())
            {
                editor.removeTag(tag);
            }

            // Add optional tags:
            editor.addTag("edit tag");

            // Add optional metadata for your own display:
            editor.setMetadata("edit meta data");

            OuyaUnityPlugin.saveOuyaMod(ouyaMod, editor);
        }
    }

    void Publish(OuyaMod ouyaMod)
    {
        OuyaUnityPlugin.contentPublish(ouyaMod);
    }

    void Unpublish(OuyaMod ouyaMod)
    {
        OuyaUnityPlugin.contentUnpublish(ouyaMod);
    }

    void Delete(OuyaMod ouyaMod)
    {
        OuyaUnityPlugin.contentDelete(ouyaMod);
    }

    void Download(OuyaMod ouyaMod)
    {
        OuyaUnityPlugin.contentDownload(ouyaMod);
    }

    void Flag(OuyaMod ouyaMod)
    {
        ouyaMod.flag();
    }

    void Rate(OuyaMod ouyaMod)
    {
        ouyaMod.rate();
    }

    void RunSearch()
    {
        m_status = "Refreshing list...";

        CleanWidgets();
        ResetFocus();

        using (OuyaContent ouyaContent = OuyaUnityPlugin.getOuyaContent())
        {
            if (null == ouyaContent)
            {
                Debug.LogError("OuyaContent is null");
                return;
            }
            if (ouyaContent.isInitialized())
            {
                OuyaUnityPlugin.getOuyaContentInstalled();
                OuyaUnityPlugin.getOuyaContentPublished(m_sortMethod);
            }
        }
    }

    void CleanWidgets()
    {
        foreach (WidgetOuyaMod widget in m_widgets)
        {
            widget.m_instance.Dispose();
            if (null != widget.m_screenshots)
            {
                foreach (Texture2D texture in widget.m_screenshots)
                {
                    if (null != texture)
                    {
                        Destroy(texture);
                    }
                }
            }
            if (null != widget.m_thumbnails)
            {
                foreach (Texture2D texture in widget.m_thumbnails)
                {
                    if (null != texture)
                    {
                        Destroy(texture);
                    }
                }
            }
        }
        m_widgets.Clear();
    }

    void AddWidgets(List<OuyaMod> ouyaMods, bool searchByInstalled, bool searchByPublished)
    {
        StringBuilder sb = new StringBuilder();
        foreach (OuyaMod ouyaMod in ouyaMods)
        {
            WidgetOuyaMod widget = new WidgetOuyaMod()
            {
                m_instance = ouyaMod,
                m_category = ouyaMod.getCategory(),
                m_description = ouyaMod.getDescription(),
                m_isDownloading = ouyaMod.isDownloading(),
                m_isFlagged = ouyaMod.isFlagged(),
                m_isInstalled = ouyaMod.isInstalled(),
                m_isPublished = ouyaMod.isPublished(),
                m_metaData = ouyaMod.getMetaData(),
                m_ratingCount = ouyaMod.getRatingCount(),
                m_ratingAverage = ouyaMod.getRatingAverage(),
                m_title = ouyaMod.getTitle(),
                m_userRating = ouyaMod.getUserRating(),
                m_searchByInstalled = searchByInstalled,
                m_searchByPublished = searchByPublished,
            };
            if (sb.Length > 0)
            {
                sb.Remove(0, sb.Length);
            }
            foreach (string filename in ouyaMod.getFilenames())
            {
                sb.Append(filename);
                sb.Append(",");

                using (InputStream inputStream = ouyaMod.openFile(filename))
                {
                    byte[] buffer = new byte[100000];
                    int readAmount = inputStream.read(ref buffer);
                    inputStream.close();

                    byte[] copy = new byte[readAmount];
                    Array.Copy(buffer, copy, readAmount);

                    sb.Append("***");
                    string content = System.Text.UTF8Encoding.UTF8.GetString(copy);
                    sb.Append(content);
                }
            }
            widget.m_filenames = sb.ToString();
            List<OuyaModScreenshot> screenshots = ouyaMod.getScreenshots();
            widget.m_screenshots = new Texture2D[screenshots.Count];
            widget.m_thumbnails = new Texture2D[screenshots.Count];
            for (int index = 0; index < screenshots.Count; ++index)
            {
                using (OuyaModScreenshot ouyaModScreenshot = screenshots[index])
                {
                    if (null != ouyaModScreenshot)
                    {
                        using (Bitmap bitmap = ouyaModScreenshot.getImage())
                        {
                            if (null != bitmap)
                            {
                                using (ByteArrayOutputStream stream = new ByteArrayOutputStream())
                                {
                                    bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream);
                                    if (stream.size() >= 0)
                                    {
                                        Texture2D texture = new Texture2D(0, 0);
                                        texture.LoadImage(stream.toByteArray());
                                        widget.m_screenshots[index] = texture;
                                    }
                                    stream.close();
                                }
                            }
                        }

                        using (Bitmap bitmap = ouyaModScreenshot.getThumbnail())
                        {
                            if (null != bitmap)
                            {
                                using (ByteArrayOutputStream stream = new ByteArrayOutputStream())
                                {
                                    bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream);
                                    if (stream.size() >= 0)
                                    {
                                        Texture2D texture = new Texture2D(0, 0);
                                        texture.LoadImage(stream.toByteArray());
                                        widget.m_thumbnails[index] = texture;
                                    }
                                    stream.close();
                                }
                            }
                        }
                    }
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(0, sb.Length);
            }
            foreach (string tag in ouyaMod.getTags())
            {
                sb.Append(tag);
                sb.Append(",");
            }
            widget.m_tags = sb.ToString();
            
            m_widgets.Add(widget);

            if (m_widgets.Count == 1)
            {
                m_focusManager.Mappings[m_btnCreate].Down = widget.m_buttonPublish;
                m_focusManager.Mappings[widget.m_buttonPublish] = new FocusManager.ButtonMapping()
                {
                    Up = m_btnCreate,
                    Right = widget.m_buttonDelete,
                };
                m_focusManager.Mappings[widget.m_buttonDelete] = new FocusManager.ButtonMapping()
                {
                    Up = m_btnCreate,
                    Left = widget.m_buttonPublish,
                    Right = widget.m_buttonDownload,
                };
                m_focusManager.Mappings[widget.m_buttonDownload] = new FocusManager.ButtonMapping()
                {
                    Up = m_btnCreate,
                    Left = widget.m_buttonDelete,
                    Right = widget.m_buttonRate,
                };
                m_focusManager.Mappings[widget.m_buttonRate] = new FocusManager.ButtonMapping()
                {
                    Up = m_btnCreate,
                    Left = widget.m_buttonDownload,
                    Right = widget.m_buttonEdit,
                };
                m_focusManager.Mappings[widget.m_buttonEdit] = new FocusManager.ButtonMapping()
                {
                    Up = m_btnCreate,
                    Left = widget.m_buttonRate,
                    Right = widget.m_buttonFlag,
                };
                m_focusManager.Mappings[widget.m_buttonFlag] = new FocusManager.ButtonMapping()
                {
                    Up = m_btnCreate,
                    Left = widget.m_buttonEdit,
                };
            }
            else
            {
                WidgetOuyaMod lastWidget = m_widgets[m_widgets.Count - 2];
                m_focusManager.Mappings[lastWidget.m_buttonPublish].Down = widget.m_buttonPublish;
                m_focusManager.Mappings[lastWidget.m_buttonDelete].Down = widget.m_buttonDelete;
                m_focusManager.Mappings[lastWidget.m_buttonDownload].Down = widget.m_buttonDownload;
                m_focusManager.Mappings[lastWidget.m_buttonRate].Down = widget.m_buttonRate;
                m_focusManager.Mappings[lastWidget.m_buttonEdit].Down = widget.m_buttonEdit;
                m_focusManager.Mappings[lastWidget.m_buttonFlag].Down = widget.m_buttonFlag;
                m_focusManager.Mappings[widget.m_buttonPublish] = new FocusManager.ButtonMapping()
                {
                    Up = lastWidget.m_buttonPublish,
                    Right = widget.m_buttonDelete,
                };
                m_focusManager.Mappings[widget.m_buttonDelete] = new FocusManager.ButtonMapping()
                {
                    Up = lastWidget.m_buttonDelete,
                    Left = widget.m_buttonPublish,
                    Right = widget.m_buttonDownload,
                };
                m_focusManager.Mappings[widget.m_buttonDownload] = new FocusManager.ButtonMapping()
                {
                    Up = lastWidget.m_buttonDownload,
                    Left = widget.m_buttonDelete,
                    Right = widget.m_buttonRate,
                };
                m_focusManager.Mappings[widget.m_buttonRate] = new FocusManager.ButtonMapping()
                {
                    Up = lastWidget.m_buttonRate,
                    Left = widget.m_buttonDownload,
                    Right = widget.m_buttonEdit,
                };
                m_focusManager.Mappings[widget.m_buttonEdit] = new FocusManager.ButtonMapping()
                {
                    Up = lastWidget.m_buttonEdit,
                    Left = widget.m_buttonRate,
                    Right = widget.m_buttonFlag,
                };
                m_focusManager.Mappings[widget.m_buttonFlag] = new FocusManager.ButtonMapping()
                {
                    Up = lastWidget.m_buttonFlag,
                    Left = widget.m_buttonEdit,
                };
            }
        }
    }

    private void Update()
    {
        if (null != m_pendingInstalledSearchResults)
        {
            AddWidgets(m_pendingInstalledSearchResults, true, false);
            m_pendingInstalledSearchResults = null;
        }
        if (null != m_pendingPublishedSearchResults)
        {
            AddWidgets(m_pendingPublishedSearchResults, false, true);
            m_pendingPublishedSearchResults = null;
        }
        UpdateFocus();
    }

    #region Focus Handling

    void ResetFocus()
    {
        m_focusManager.Mappings.Clear();
        m_focusManager.Mappings[m_btnSearchByRating] = new FocusManager.ButtonMapping()
        {
            Down = m_btnSearchByCreatedAt,
        };
        m_focusManager.Mappings[m_btnSearchByCreatedAt] = new FocusManager.ButtonMapping()
        {
            Up = m_btnSearchByRating,
            Down = m_btnSearchByUpdatedAt,
        };
        m_focusManager.Mappings[m_btnSearchByUpdatedAt] = new FocusManager.ButtonMapping()
        {
            Up = m_btnSearchByCreatedAt,
            Down = m_btnCreate,
        };
        m_focusManager.Mappings[m_btnCreate] = new FocusManager.ButtonMapping()
        {
            Up = m_btnSearchByUpdatedAt,
        };

        // set default selection
        m_focusManager.SelectedButton = m_btnCreate;
    }

    private void UpdateFocus()
    {
        if (GetButtonUp(OuyaController.BUTTON_DPAD_DOWN))
        {
            m_focusManager.FocusDown();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_LEFT))
        {
            m_focusManager.FocusLeft();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_RIGHT))
        {
            m_focusManager.FocusRight();
        }
        if (GetButtonUp(OuyaController.BUTTON_DPAD_UP))
        {
            m_focusManager.FocusUp();
        }
    }

    public class FocusManager
    {
        private const int DELAY_MS = 150;

        public object SelectedButton = null;

        private void SetSelection(object selection)
        {
            if (null != selection)
            {
                SelectedButton = selection;
            }
        }

        public class ButtonMapping
        {
            public object Up = null;
            public object Left = null;
            public object Right = null;
            public object Down = null;
        }

        public Dictionary<object, ButtonMapping> Mappings = new Dictionary<object, ButtonMapping>();

        public void FocusDown()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Down);
            }
        }
        public void FocusLeft()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Left);
            }
        }
        public void FocusRight()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Right);
            }
        }
        public void FocusUp()
        {
            if (null != SelectedButton &&
                Mappings.ContainsKey(SelectedButton))
            {
                SetSelection(Mappings[SelectedButton].Up);
            }
        }
    }

    #endregion
#endif
}
