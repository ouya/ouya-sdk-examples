#import <AdSupport/ASIdentifierManager.h>

extern "C"
{
    char* GetUserID()
    {
        NSString *uidString = @"";
        
        if (!NSClassFromString(@"ASIdentifierManager"))
        {
            uidString = @"OLD";
        }
        else
        { //iOS6+
            if([[ASIdentifierManager sharedManager] isAdvertisingTrackingEnabled])
            {
                uidString = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
            }
        }
        
        const char* string = [uidString UTF8String];
        
        if (string == NULL)
            return NULL;
        
        char* res = (char*)malloc(strlen(string) + 1);
        strcpy(res, string);
        
        return res;
    }
}