# Robot Repair — Кравець Арсен

Навчальний проєкт з дисципліни «Геймдизайн та програмування ігрових застосунків». РФКІТ, 2026/27.

## Проєкт
Adventure Game: Robot Repair (Unity Technologies), Unity 6.3, URP (Universal 2D). Набір артів — **Ruby**.

## Як запустити
Відкрити теку проєкту через Unity Hub, редактор 6.3, відкрити сцену `Assets/_2DAdventureGame/Scenes/MainScene`, натиснути Play.

**Керування:** стрілки або WASD (або лівий стік геймпада) — рух, **C** — кинути шестерню, **X** — поговорити з NPC.
Мета: полагодити всіх зламаних роботів шестернями. Якщо здоров'я впаде до нуля — поразка, рівень починається спочатку.

## Що зроблено
- Лабораторна 1: створено проєкт Universal 2D, імпортовано Robot Repair, перемкнуто набір артів, налаштовано репозиторій з `.gitignore` для Unity.
- Лабораторна 2 (юніт 1): сцена MainScene, персонаж PlayerCharacter, скрипт PlayerController, тайлмап з Rule Tile (FirstTile, SecondTile), палітра Game Palette, рух через Input Action (стрілки + WASD), Time.deltaTime.
- Лабораторна 3 (юніт 2): рівень з тайлсетів (нарізка 3×3, PPU 64), декорації з Pivot = Bottom і сортуванням за Y (Custom Axis 0,1,0), префаби, Rigidbody2D + Box Collider 2D, рух через MovePosition у FixedUpdate, Tilemap Collider 2D + Composite Collider 2D (вода непрохідна).
- Лабораторна 4 (юніт 3): здоров'я (maxHealth, властивість health, ChangeHealth з Mathf.Clamp), швидкість у Inspector, аптечки (HealthCollectible, тригер), зони шкоди (DamageZone, OnTriggerStay2D, Never Sleep), невразливість після удару.
- Лабораторна 5 (юніт 4): ворог з патрулем за таймером (EnemyController), анімації (EnemyLeft, EnemyRight з Flip X, EnemyFixed, Blend Tree 2D Simple Directional), анімації робота (Look X/Y, Speed, Hit, Launch), снаряд (Projectile, AddForce), шари Character/Projectiles, полагодження ворога (Fix).
- Лабораторна 6 (юніт 5): HUD у UI Toolkit (GameUI.uxml, смуга здоров'я, UIHandler-синглтон), NPC з анімацією, Raycast і бульбашкою «Press X», вікно репліки (NPCDialogue.uxml), екрани перемоги й поразки (EndScreen.uxml), GameManager з перезапуском сцени.

## Щоденник розробника

### 29.10.2026 — Лабораторна 1: знайомство з редактором
Що зроблено: створив проєкт Universal 2D на Unity 6.3, через Package Manager → My Assets імпортував Robot Repair, вибрав набір артів Ruby, додав `.gitignore` для Unity і README.
Що не вийшло або забрало найбільше часу: імпорт пакета як цілого проєкту — Unity попереджає, що перезапише ProjectSettings, і довго ставить залежності.
Що зрозумів: Library, Temp і Logs у репозиторій не потрапляють — їх Unity генерує сам.
Що робитиму далі: власна сцена і перший скрипт.

### 29.10.2026 — Юніт 1: персонаж і рух
Що зроблено: сцена MainScene, PlayerCharacter, скрипт PlayerController, підлога з Rule Tile FirstTile і SecondTile, палітра Game Palette, MoveAction з двома композитами (стрілки і WASD) та стіком геймпада.
Що не вийшло або забрало найбільше часу: між плитками були щілини, поки Tile1 і Tile2 мали PPU 100 замість 64; підлога малювалась поверх персонажа, поки в Tilemap Renderer не поставив Order in Layer = −10.
Що зрозумів про Update і Time.deltaTime: Update викликається щокадру, тому без множення на Time.deltaTime швидкість залежала б від FPS; 3.0f * Time.deltaTime — це три одиниці за секунду на будь-якому комп'ютері.
Що робитиму далі: колізії, щоб робот не ходив крізь воду і ящики.

### 29.10.2026 — Юніт 2: оточення і фізика
Що зроблено: нарізав тайлсети підлоги й води на 9 спрайтів, намалював рівень з цегляною площею і двома ставками, розставив ящики, кущі й дерева (префаби з колайдером на нижній половині), робот отримав Rigidbody 2D (Gravity Scale 0, Freeze Rotation Z) і Box Collider 2D на ногах.
Що не вийшло або забрало найбільше часу: робот тремтів біля ящика, поки рух ішов через transform.position, — переніс у FixedUpdate з rigidbody2d.MovePosition.
Що зрозумів про FixedUpdate і колайдери: ввід читаємо в Update, тіло рухаємо у FixedUpdate — у такті фізики. Тайлам підлоги Collider Type = None, а вода з Composite Collider 2D (Merge) дає один гладкий контур.
Що в рівні вийшло не так, як у фізичному паспорті: камера в нас нерухома, тому довелося додати невидиму межу рівня (Edge Collider 2D), щоб робот не виходив за кадр.
Що робитиму далі: здоров'я, аптечки й зони шкоди.

### 29.10.2026 — Юніт 3: система здоров'я
Що зроблено: maxHealth = 5, властивість health лише для читання, ChangeHealth з Mathf.Clamp, speed у Inspector, аптечки-тригери, дві зони шкоди з Draw Mode = Tiled і Auto Tiling, невразливість після удару, Sleeping Mode = Never Sleep.
Що не вийшло або забрало найбільше часу: без невразливості зона забирала все здоров'я за частку секунди (OnTriggerStay2D викликається 50 разів на секунду).
Що зрозумів про тригери, public і private: тригер не зупиняє тіло, а лише викликає OnTriggerEnter2D/Stay2D; currentHealth лишився private — змінити здоров'я можна тільки через ChangeHealth.
Яке значення Time Invincible обрав і для якого гравця: 1.5 с — гравець встигає вийти із зони, але помилка все одно відчутна.
Що робитиму далі: вороги та снаряди.

### 29.10.2026 — Юніт 4: вороги і снаряди
Що зроблено: префаб Enemy (тригер, Rigidbody 2D), патруль за таймером горизонтально й вертикально, кліпи EnemyLeft/EnemyRight/EnemyFixed (Samples = 4), Blend Tree за Move X / Move Y, анімації робота через Look X, Look Y, Speed, Hit, Launch, снаряд-шестерня на клавішу C, шари Character і Projectiles без колізії між собою.
Що не вийшло або забрало найбільше часу: снаряд зникав одразу, бо народжувався в колайдері робота, — допомогла Layer Collision Matrix.
Що зрозумів: Awake виконується ще всередині Instantiate, тому rigidbody2d снаряда вже є, коли викликається Launch.
Мої числа: три вороги — Speed 1 / Change Time 4, Speed 1 / Change Time 3 (вертикальний), Speed 1.5 / Change Time 3; сила пострілу 300.
Що робитиму далі: HUD, NPC і кінець гри.

### 29.10.2026 — Юніт 5: інтерфейс, NPC і кінець гри
Що зроблено: GameUI.uxml зі смугою здоров'я і портретом, Panel Settings (Scale With Screen Size, 1920×1080), UIHandler-синглтон, NPC з анімацією та Raycast на шар NPC, бульбашка «Press X», вікно репліки на 4 секунди, екрани перемоги й поразки з плавною появою, GameManager з перезапуском сцени, MainScene додано у Build Profiles.
Що не вийшло або забрало найбільше часу: без (float) у currentHealth / (float)maxHealth смуга була б або повна, або порожня — ділення цілих.
Що зрозумів: Q<VisualElement>("HealthBar") шукає елемент за ім'ям дослівно; Opacity + Transition Animations дає плавну появу екрана.
Що робитиму далі: прапорець gameOver, щоб Invoke не ставив перезапуск у чергу щокадру, і камера, що йде за роботом.
