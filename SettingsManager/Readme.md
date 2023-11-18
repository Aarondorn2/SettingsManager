# Introduction

Welcome! Below is a brief description of what SettingsManager is and how it works. SettingsManager is meant to be a light-weight
library that organizes and standardizes how settings, configurations, and features are stored and accessed. It allows you to bring
your own persistence provider (a database or similar datastore is recommended).

The overall concept is simple: a C# class is defined that implements either an IFeature, IConfig, or ISetting (more specifics below).
That class is converted to JSON and persisted. Later, the setting can be retrieved and serialized back into the C# object. The JSON
serialization allows you to have both complex nested configuration objects while simultaneously keeping your persistence provider
simple and clean. The configurations are stored with a compound key comprised of a customerId (or globalId if the customer context
is not required) and the FQDN of the class that stores the configuration.

Some application settings and configurations hold sensitive data. To account for this, this library contains functionality
that allows individual properties to be encrypted prior to persistence, and then subsequently decrypted when accessed. A simple
annotation can be added to any property that needs this functionality and the framework will handle the rest. An AES provider
is included in the project (requires you add your own symmetric key) but you may use any algorithm you wish.

This pattern also allows for a default configuration to be persisted as well. If a setting is not configured for a specific customer
or uer, then the default setting can be loaded. There are separate service methods defined for situations where you do and do not
want to fall-back to the default settings.

For organizational simplicity, it is recommended that you store all of your configuration objects in a common directory, 
although this is not required for this library to function. Note that moving settings classes to different namespaces after
they have been persisted will cause the key to change and thus the persisted data must also be updated. A recommended
structure can be illustrated as follows, accounting for the different types of settings:

* Settings
  * Configs
  * Features

The difference in types can be described as such:

Config - A collection of similar properties that affect application behavior but apply globally regardless of which customer or
user is interacting with the application.

Setting - A collection of similar properties that affect application behavior that may change depending on the customer or
user interacting with the application.

Feature - An application function that can be enabled/disabled based on the customer or user interacting with the application.

The SettingsManager pattern has been a labor of love developed, refined, and used in enterprise applications over the past ~6 years.
Many contributors have added to and improved the concept over the years. It has been implemented on different platforms and tech
stacks and is now available as open source.

If you have questions, suggestions, or feedback, please feel free to open a github issue.


# Example Usage

SettingsManager is a pattern for persisting and accessing any type of setting in a standardized way. Here is an example of how
settings can be accessed:

```
// note: when your application starts up, you must call `SettingsManager.Init()` and pass it the providers you wish
// to use for persistence and encryption (as well as JSON serialization options if desired). This call is required before
// you call any methods on the SettingsManagerService
    
public class MySetting : ISetting
{
    public string? MyString { get; set; }
}

public class MyController
{
    private ISettingsManagerService _settingsManager;
    
    publif MyController(ISettingsManagerService settingsManager)
    {
        _settingsManager = settingsManager;
    }
    
    public void MyMethod(Guid customerId)
    {
        var mySetting = _settingsManager.GetSetting<MySetting>(customerId);
        Console.WriteLine(mySetting.MyString);
    }
}
```
