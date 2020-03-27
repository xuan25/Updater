# Updater

**A automatic updater solution based on Github api**

## Config

To use this updater you need to setup a numbers of variables

### Client

In **UpdateUtil.cs**

```C#
// The UserAgent field when requesting to the Github Api server.
private const string UserAgent;

// The repository of the release page.
// Format is "[Username]/[Repository name]" (for example: "xuan525/Updater" for current repo)
private const string RepoName;

// The Uri of the updater in program resources
// For example: "Updater/updater.exe" for provided demo
private const string UpdaterUri

// The path where updater will been released on the disk.
// For example: AppDomain.CurrentDomain.BaseDirectory + "updater.exe"
private static string UpdaterPath;

// Current version code
public static string CurrentVersion;
```

### Updater

In **Downloader.cs**

```C#
// The UserAgent field when requesting to the Github Api server.
private const string UserAgent;

// The repository of the release page.
// Format is "[Username]/[Repository name]" (for example: "xuan525/Updater" for current repo)
private const string RepoName;
```
