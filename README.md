# Dotbot.Telegram

An adapter for the [Dotbot framework](https://github.com/botnetcore/dotbot) for [Telegram](https://telegram.org/).

Powered by [Telegram.Bot](https://github.com/MrRoundRobin/telegram.bot) by Robin Muller.

## Getting Started

To add the Telegram adapter to your bot, just update your startup code: 

```csharp
// Build the robot.
var robot = new RobotBuilder()
    // ...
    .UseTelegram("MY_TELEGRAM_TOKEN")
    // ...
    .Build();
```