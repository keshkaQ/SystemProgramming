using System.Runtime.InteropServices;

class Program
{
    // [DllImport] - атрибут, указывающий на то, что метод вызывается из внешней библиотеки DLL (user32.dll).
    // CharSet - используются строки в кодировке Unicode (с поддержкой кириллицы).
    // Объявляем импорт функции MessageBoxW из user32.dll
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBoxW(
        IntPtr hWnd,           // Дескриптор родительского окна (IntPtr.Zero — нет родителя)
        string lpText,         // Текст сообщения
        string lpCaption,      // Заголовок окна
        uint uType             // Тип сообщения (кнопки, иконка и т.д.)
    );

    static void Main()
    {
        // Вызываем функцию MessageBoxW  - это встроенная функция Windows для отображения окна с сообщением.
        MessageBoxW(IntPtr.Zero, "Привет из user32.dll!", "Сообщение", 0x00000040);
    }
}