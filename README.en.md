# WPF TreeView Control Library Usage Guide

## Overview

**Developers have long suffered from TreeView in WPF!**
From overly complex custom templates to complicated data bindings, building a fully functional tree component has always been exhausting.
And to this day, there is still no unified, complete, and easy-to-use TreeView wrapper.

**Now I present a practical solution!**

This project provides a complete set of object-oriented features for tree node operations, freeing developers from the burden of templates, bindings, and event wiring — so you can focus on business logic.

---

## 🔗 Repository & NuGet

* NuGet: **Su.WPF.TreeViewEx**
* Gitee: [https://gitee.com/SususuChang/su.-wpf.-custom-control](https://gitee.com/SususuChang/su.-wpf.-custom-control)
* GitHub: [https://github.com/ViewSuSu/Su.WPF.TreeViewEx](https://github.com/ViewSuSu/Su.WPF.TreeViewEx)

---

## 📦 NuGet Installation

### Package Manager Console

```powershell
Install-Package Su.WPF.TreeViewEx
```

### .NET CLI

```bash
dotnet add package Su.WPF.TreeViewEx
```

### Visual Studio NuGet Manager

1. Right-click your project → **Manage NuGet Packages**
2. Search for `Su.WPF.TreeViewEx`
3. Click **Install**

### csproj Reference

```xml
<PackageReference Include="Su.WPF.TreeViewEx" Version="1.0.0" />
```

---

## Framework Support

This library works with:

* **.NET Framework 4.5+**
* **.NET Core 3.1+**
* **.NET 5.0 (Windows Desktop only) and later**


---

## 🎬 Demo Animation

<div align="left">
<img src="./HD.gif" alt="TreeViewEx Feature Demo" width="30%">
</div>

---

## 🎯 Core Design Principles

### 🏗️ Robust Architecture Based on SOLID Principles

Strict object-oriented design with strong emphasis on the **Open-Closed Principle**:

* **✅ Open for extension** — E.g. inherit from `TreeNodeEx`, `MenuBase` to extend features
* **✅ Closed for modification** — API remains stable across versions
* **✅ Single Responsibility** — Clear boundary of class duties
* **✅ Interface Segregation** — Use only what you need

---

### 🌟 Pure Implementation with Zero Dependencies

**No dependency on any third-party frameworks!**

```csharp
// 100% native WPF
// ✅ No MVVM frameworks (Prism, MVVMLight...)
// ✅ No UI libraries (MaterialDesign, MahApps...)
// ✅ No IoC containers (Autofac, Unity...)
// ✅ No additional NuGet dependencies

// Absolutely plug-and-play
```

---

### 🔒 Safe API Design

```csharp
// Compiler prevents wrong usage
// ❌ These cause compile-time errors:
// node.MenuItems.Add(...);
// provider.Controller = null;
// node.Children = new List<>();  

// ✅ Only allowed via designed public methods:
node.AddChild("Safe operation");
node.MenuItemModels.Add(menu);
```

---

## 🚧 Current Status

The core features are **stable and production-ready**, while further improvements are planned.

### ✅ Features Already Implemented

* Full tree structure operations (create/delete/traverse)
* Checkbox system — three-state automatic cascade
* Full context menu support with shortcuts
* Styling system — icons, colors, fonts...
* Advanced selection tracking (multi-select)
* Deep copy support
* Data binding to custom models

### ⚠️ Planned Enhancements

#### Performance Improvements

* UI Virtualization support for fast rendering of 1000+ nodes
* Better memory lifecycle management

#### Async Operation Support

* Async loading for large/remote data
* Async batch node operation

#### Future Advanced Features

* Lazy loading sub-trees
* Expand/collapse animations
* Drag-and-drop
* Real-time filtering & searching

---

## 🚀 Quick Start

### 1️⃣ Add to XAML

```xml
<Grid>
    <ContentControl Content="{Binding Provider.TreeView, Mode=OneWay}" />
</Grid>
```

### 2️⃣ Basic ViewModel Setup

```csharp
using Su.WPF.CustomControl.TreeViewEx;
using Su.WPF.CustomControl.Menu;

public class MainWindowViewModel
{
    public TreeViewExProvider Provider { get; }
    public List<TreeNodeEx> TreeNodeExs { get; set; }

    public MainWindowViewModel()
    {
        InitializeTreeNodes();
        Provider = TreeViewExProvider.GetTreeViewPanelProvider(TreeNodeExs);
        ConfigureMenus();
    }

    private void InitializeTreeNodes()
    {
        TreeNodeExs = new List<TreeNodeEx>
        {
            TreeNodeEx.CreateNode("Root")
        };
    }

    private void ConfigureMenus()
    {
        // Configure menus here
    }
}
```

---

## 🎨 Core Usage Examples

### 1️⃣ Create Tree Structure

```csharp
var root = TreeNodeEx.CreateNode("Project");
var src = root.AddChild("Source");
src.AddChild("MainWindow.xaml.cs");
src.AddChild("MainViewModel.cs");

root.AddRange(new[] {
    TreeNodeEx.CreateNode("Docs"),
    TreeNodeEx.CreateNode("Assets")
});
```

### 2️⃣ Icon Styling

```csharp
var node = TreeNodeEx.CreateNode("Icon Node");
node.TreeNodeExIconOptions.Icon = yourImage;
node.TreeNodeExIconOptions.Width = 20;
```

### 3️⃣ Text Styling

```csharp
titleNode.TreeNodeExTextOptions.FontSize = 16;
titleNode.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;
```

### 4️⃣ Checkbox System

```csharp
var p = TreeNodeEx.CreateNode("Parent");
p.IsShowCheckBox = true;

var c = p.AddChild("Child");
c.IsShowCheckBox = true;
c.IsChecked = true;
```

### 5️⃣ Shortcut-Enabled Menus

#### Tree-level menu

```csharp
var refreshMenu = new TreeViewMenu("Refresh", RefreshAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.R)
};

provider.Controller.Options.MenuItemModels.Add(refreshMenu);
```

#### Node-level menu

```csharp
fileNode.MenuItemModels.Add(
    new TreeNodeMenu("Open", n => OpenFile(n))
);
```

### 6️⃣ Query API

```csharp
var copy = source.Copy();
var checkedNodes = parent.GetAllCheckedDescendants();
```

---

## 📋 API Reference

### Node Operations

| Method                        | Description                 |
| ----------------------------- | --------------------------- |
| `TreeNodeEx.CreateNode(text)` | Create a node               |
| `AddChild(...)`               | Add child node              |
| `AddRange(list)`              | Batch add children          |
| `Copy()`                      | Deep copy node with subtree |
| `CopyTo(parent)`              | Copy under another node     |
| `Delete()`                    | Remove node                 |

### Checkbox Queries

| Method                       | Meaning                    |
| ---------------------------- | -------------------------- |
| `GetCheckedChildren()`       | Direct checked children    |
| `GetAllCheckedDescendants()` | All checked descendants    |
| `HasCheckedChildren()`       | Any checked children exist |

### Menu API

| Feature         | Description                         |
| --------------- | ----------------------------------- |
| Tree-level menu | `Controller.Options.MenuItemModels` |
| Node-level menu | `node.MenuItemModels`               |
| Menu shortcut   | `MenuShortcut`                      |

---

## 🎯 Best for These Scenarios

✅ Up to 500 nodes
✅ Local and synchronous data
✅ Full context menu + checkboxes
✅ Custom icons & styles
✅ Multi-selection

---

## Summary

**Powerful, stable, and developer-friendly — right out of the box!**
Perfect for 90% of hierarchical UI scenarios.

---

## 💡 Design Philosophy

> **“Simple should not be made complicated.
> Complex should not be oversimplified.”**

---

