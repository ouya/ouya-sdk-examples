purchasables[0] = "long_sword";
purchasables[1] = "sharp_axe";
purchasables[2] = "cool_level";
purchasables[3] = "awesome_sauce";
purchasables[4] = "__DECLINED__THIS_PURCHASE";

strPurchasables = purchasables[0];
for (index = 1; index < array_length_1d(purchasables); ++index)
{
    strPurchasables += ","+purchasables[index];
}

init_ouya_plugin_values = ds_map_create();
ds_map_add(init_ouya_plugin_values, "tv.ouya.developer_id", "310a8f51-4d6e-4ae5-bda0-b93878e5f5d0");
ds_map_add(init_ouya_plugin_values, "com.xiaomi.app_id", "2882303761517238170");
ds_map_add(init_ouya_plugin_values, "com.xiaomi.app_key", "5571723882170");
ds_map_add(init_ouya_plugin_values, "tv.ouya.product_id_list", strPurchasables);
OuyaSDK_Init(json_encode(init_ouya_plugin_values));
ds_map_destroy(init_ouya_plugin_values);

button_index = 0;
text_message = "Status:";
controllers_active = "";

products_index = 0;
products_length = 0;
products[0] = "";
product_ids[0] = "";

receipts_length = 0;
receipts[0] = "";