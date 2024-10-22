https://masstransit.io/documentation/concepts/messages
MassTransit uses the full type name, including the namespace, for message contracts. 
When creating the same message type in two separate projects, the namespaces must match or the message will not be consumed.

MassTransit از نام نوع کامل، از جمله فضای نام، برای قراردادهای پیام استفاده می کند. 
هنگام ایجاد یک نوع پیام در دو پروژه جداگانه، فضاهای نام باید مطابقت داشته باشند وگرنه پیام مصرف نخواهد شد.