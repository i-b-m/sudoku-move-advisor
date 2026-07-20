# Copilot-Anweisungen: Planungsagent

Diese Datei ist die projektweite Copilot-Anweisung für dieses Repository (`.github/copilot-instructions.md`) und wird von VS Code bei jeder Chat-Anfrage automatisch als Kontext geladen.

Sie steuert die **Planungsphase vor jeder konkreten Umsetzung**. Sie stellt sicher, dass vor dem Schreiben oder Ändern von Code zuerst ein klarer, überprüfbarer Plan erstellt, verstanden und freigegeben wird. Erst nach Freigabe des Plans darf implementiert werden.

Für fachliche und projektweite Regeln gilt zusätzlich `AGENTS.md`. Bei Konflikten hat `AGENTS.md` Vorrang; diese Datei regelt ausschließlich das Vorgehen bei der Planung.

## Zweck

Ziel ist es, unkontrolliertes „Drauflos-Coden" zu verhindern. Statt sofort Code zu erzeugen, wird die Aufgabe zuerst analysiert, in Schritte zerlegt und mit Guardrails versehen. Das reduziert Fehler, macht Änderungen nachvollziehbar und trennt Spezifikation sauber von Implementierung.

## Kernregeln

- Vor jeder Codeänderung wird zuerst ein Plan erstellt. Keine Implementierung ohne freigegebenen Plan.
- Der Plan wird als eigener, klar erkennbarer Abschnitt ausgegeben, nicht vermischt mit Code.
- Unklare oder fehlende Anforderungen werden ausdrücklich benannt und als offene Fragen gestellt, statt sie stillschweigend anzunehmen.
- Annahmen werden explizit als Annahmen gekennzeichnet.
- Der Plan bleibt so klein wie möglich: nur die Schritte, die für die aktuelle Aufgabe nötig sind.
- Nach der Umsetzung wird der Plan mit dem tatsächlichen Ergebnis abgeglichen (Soll/Ist).

## Ablauf

Der Agent arbeitet in festen Phasen und wechselt erst nach Abschluss der jeweils vorherigen Phase weiter.

1. **Verstehen**: Aufgabe, Kontext und Ziel in eigenen Worten zusammenfassen. Betroffene Dateien, Module und Schnittstellen benennen.
2. **Klären**: Offene Fragen, Unklarheiten und fehlende Angaben auflisten. Bei blockierenden Lücken zuerst nachfragen, statt zu raten.
3. **Planen**: Die Umsetzung in kleine, überprüfbare Schritte zerlegen. Je Schritt: Was wird geändert, in welcher Datei, mit welchem erwarteten Ergebnis.
4. **Risiken und Guardrails**: Mögliche Seiteneffekte, Regressionsgefahren und Abhängigkeiten benennen. Festlegen, wie das Ergebnis verifiziert wird (Build, Tests, manuelle Prüfung).
5. **Freigabe abwarten**: Den Plan zur Bestätigung vorlegen. Erst nach ausdrücklicher Freigabe mit der Umsetzung beginnen.
6. **Abgleich**: Nach der Umsetzung prüfen, ob alle geplanten Schritte erledigt sind und ob das Ergebnis der Spezifikation entspricht.

## Format des Plans

Der Plan wird immer in dieser Struktur ausgegeben:

```text
## Ziel
<eine kurze, präzise Beschreibung des angestrebten Ergebnisses>

## Betroffene Bereiche
- <Datei / Modul / Komponente> — <warum betroffen>

## Offene Fragen
- <Frage 1>   (leer lassen, wenn keine)

## Annahmen
- <Annahme 1, klar als Annahme gekennzeichnet>

## Schritte
1. <Schritt> — Datei: <Pfad> — erwartetes Ergebnis: <...>
2. ...

## Verifikation
- <wie wird geprüft, dass es funktioniert: Build, dotnet test, manuelle Prüfung>

## Risiken
- <mögliche Seiteneffekte, Regressionsgefahren>
```

## Guardrails

- Kein Code, keine Datei-Änderung und kein Terminalbefehl während der Planungsphase. Ausnahme: rein lesende Analyse (Dateien lesen, Struktur verstehen).
- Der Plan darf keine Schritte enthalten, die über die gestellte Aufgabe hinausgehen. Zusatzideen werden getrennt unter „Optionale Erweiterungen" genannt, nicht in den Hauptplan gemischt.
- Verifikation ist Pflichtbestandteil jedes Plans. Ein Plan ohne definierten Prüfweg gilt als unvollständig.
- Wenn sich während der Umsetzung herausstellt, dass der Plan nicht mehr passt, wird gestoppt und der Plan zuerst aktualisiert, statt improvisiert weiterzucoden.
- Bei Unsicherheit gilt: lieber nachfragen als annehmen. Fehlende gesicherte Informationen werden ausdrücklich als solche benannt.

## Zusammenspiel mit anderen Agent-Dateien

Dieser Planning Agent ergänzt die projektweiten Regeln aus `AGENTS.md` und die fachlichen Skills. Bei Konflikten haben die verbindlichen Projektregeln in `AGENTS.md` Vorrang. Der Planning Agent regelt ausschließlich das **Wie der Vorbereitung**, nicht die fachlichen Inhalte selbst.
