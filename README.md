# 🏫 School Planner (C# / .NET)

**School Planner** è un'applicazione desktop sviluppata in **C#** dedicata alla gestione completa della carriera scolastica. Il software permette di organizzare l'orario, memorizzare i voti e calcolare le medie in modo locale e sicuro.

---

## 🛠️ Caratteristiche Tecniche

- **Linguaggio**: C#
- **Framework**: .NET (Windows Forms / WPF)
- **Archiviazione**: [Scegli tra: SQL Server / SQLite / JSON file]
- **Paradigma**: Programmazione Orientata agli Oggetti (OOP) con classi dedicate per `Studente`, `Materia` e `Voto`.

---

## 🌟 Funzionalità

- 📅 **Orario Settimanale**: Visualizzazione tabellare delle lezioni giornaliere.
- 📈 **Gestione Medie**: Algoritmo in C# per il calcolo automatico della media aritmetica e ponderata.
- 📁 **Salvataggio Dati**: Serializzazione dei dati per mantenere le informazioni salvate alla chiusura del programma.
- 🔍 **Filtri Materia**: Ricerca rapida delle valutazioni per singola disciplina.

---

## 💻 Requisiti di Sistema

- Sistema Operativo: Windows 10/11
- Ambiente di Sviluppo: **Visual Studio 2022** (o VS Code con C# Dev Kit)
- Runtime: .NET SDK installato

---

## 🚀 Come avviare il progetto

1. Apri il file della soluzione `.sln` con **Visual Studio**.
2. Ripristina i pacchetti NuGet (se presenti).
3. Premi **F5** o il tasto "Avvia" per compilare ed eseguire l'applicazione.

---

## 📌 Struttura del Codice (Esempio Classi)

```csharp
public class Materia {
    public string Nome { get; set; }
    public List<double> Voti { get; set; } = new List<double>();
    
    public double CalcolaMedia() {
        if (Voti.Count == 0) return 0;
        return Voti.Average();
    }
}
