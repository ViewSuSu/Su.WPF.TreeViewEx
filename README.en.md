# WPF TreeView Control Wrapper Library – Usage Guide

## Overview

**Developers have long struggled with TreeView!** Online resources are flooded with various complex solutions, from intricate custom templates to cumbersome data bindings. Implementing a fully functional TreeView in WPF has been a challenge for many. However, until now, there hasn’t been a unified, complete, and easy-to-use solution.

**Here’s a practical solution!**

This project provides a complete, object-oriented approach to tree node management, freeing developers from tedious template definitions, data bindings, and event handling, allowing them to focus solely on business logic.

## Framework Support

This wrapper library supports the following frameworks:

* **.NET Framework 4.5 and above**
* **.NET Core 3.1 and above**
* **.NET 5.0 (Windows desktop only) and above**

> This means you can use it directly in WPF or WinForms desktop applications without additional adaptation.

## 🎬 Demo Animation

<div align="left">
<img src="./HD.gif" alt="TreeViewEx Demo" width="30%">
</div>

## 🎯 Core Design Philosophy

### 🏗️ Robust Architecture Based on SOLID Principles

The project follows **object-oriented design** and strictly adheres to **SOLID principles**, especially the **Open-Closed Principle**:

* **✅ Open for extension**: Easily inherit from base classes like `TreeNodeEx` or `MenuBase` to add custom functionality.
* **✅ Closed for modification**: Core APIs are stable; updates won’t break existing code.
* **✅ Single Responsibility**: Each class has a clear responsibility, avoiding "god objects".
* **✅ Interface Segregation**: Provides fine-grained configuration options for selective use.

### 🌟 Zero-Dependency Implementation

**Key feature: no third-party dependencies!**

```csharp
// Pure native WPF implementation
// ✅ No dependency on MVVM frameworks (Prism, MVVMLight, etc.)
// ✅ No dependency on UI libraries (MaterialDesign, MahApps, etc.)
// ✅ No dependency on IoC containers (Autofac, Unity, etc.)
// ✅ No other NuGet packages required

// Plug-and-play, no complex setup needed
```

### 🔒 Safe API Design

```csharp
// The compiler prevents incorrect usage
// ❌ These will cause compile-time errors:
// node.MenuItems.Add(...);        // MenuItems is read-only
// provider.Controller = null;     // Controller is read-only
// node.Children = new List<>();   // Children collection is protected

// ✅ Only allowed through designated public methods
node.AddChild("Safe operation");
node.MenuItemModels.Add(menu);     // Correct collection operation
```

## 🚧 Current Version Status

**Optimization is ongoing!** The current version already implements the core features, providing a stable foundation, but improvements are planned:

### ✅ Stable Features Implemented

* **Complete tree structure management** – node creation, deletion, traversal, etc.
* **Checkbox system** – tri-state management, automatic cascading, state queries
* **Context menu system** – tree-level and node-level menus, with shortcut support
* **Style configuration system** – icons, colors, fonts, and other visual customization
* **Selection management** – multi-selection support and state tracking
* **Copy functionality** – complete node structure copy
* **Data binding** – linking to custom data objects

### ⚠️ Known Areas for Improvement

#### 1. **Performance Optimization**

* **Virtualization**: Current version may experience performance bottlenecks with large datasets (1000+ nodes). Future versions will implement virtualization, rendering only visible nodes to improve performance.
* **Memory management**: Optimize node lifecycle to reduce memory footprint.

#### 2. **Asynchronous Support**

* **Data loading**: All current operations are synchronous and may block the UI when handling large or remote datasets.
* **Batch operations**: Implement async batch node operations for better UX.

#### 3. **Advanced Features Planned**

* **Lazy loading**: Load child nodes only when needed.
* **Animation**: Smooth expand/collapse node animations.
* **Drag-and-drop support**: Full drag-and-drop functionality.
* **Filter & search**: Real-time filtering and searching.

## 🚀 Quick Start

### 1. XAML Configuration

```xml
<Grid>
    <ContentControl Content="{Binding Provider.TreeView, Mode=OneWay}" />
</Grid>
```

### 2. ViewModel Structure

```csharp
public class MainWindowViewModel
{
    public TreeViewExProvider Provider { get; }
    public List<TreeNodeEx> TreeNodeExs { get; set; }

    public MainWindowViewModel()
    {
        // 1. Initialize tree nodes
        InitializeTreeNodes();
        
        // 2. Get tree view provider
        Provider = TreeViewExProvider.GetTreeViewPanelProvider(TreeNodeExs);
        
        // 3. Configure menus
        ConfigureMenus();
    }

    private void InitializeTreeNodes()
    {
        TreeNodeExs = new List<TreeNodeEx>
        {
            TreeNodeEx.CreateNode("Root Node")
        };
    }

    private void ConfigureMenus()
    {
        // Configure tree-level and node-level menus
    }
}
```

## 🎨 Core Features Usage

### 1. **Basic Tree Structure**

```csharp
var root = TreeNodeEx.CreateNode("My Project");

var srcFolder = root.AddChild("Source Code");
srcFolder.AddChild("MainWindow.xaml.cs");
srcFolder.AddChild("MainViewModel.cs");

root.AddRange(new[] {
    TreeNodeEx.CreateNode("Documents"),
    TreeNodeEx.CreateNode("Resources")
});
```

### 2. **Icon Configuration**

```csharp
var node = TreeNodeEx.CreateNode("Node with Icon");
node.TreeNodeExIconOptions.Icon = yourImageSource;
node.TreeNodeExIconOptions.Width = 20;
```

### 3. **Text Style Configuration**

```csharp
var titleNode = TreeNodeEx.CreateNode("Title");
titleNode.TreeNodeExTextOptions.FontSize = 16;
titleNode.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;
```

### 4. **Checkbox System**

```csharp
var parent = TreeNodeEx.CreateNode("Parent Node");
parent.IsShowCheckBox = true;

var child = parent.AddChild("Child Node");
child.IsShowCheckBox = true;
child.IsChecked = true;
```

### 5. **Menu System & Shortcuts**

#### Tree-Level Menu

```csharp
var refreshMenu = new TreeViewMenu("Refresh", RefreshAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.R)
};

var saveMenu = new TreeViewMenu("Save", SaveAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control | ModifierKeys.Shift, Key.S)
};

var advancedMenu = new TreeViewMenu("Advanced", AdvancedAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Alt | ModifierKeys.Control, Key.F1)
};

provider.Controller.Options.MenuItemModels.Add(refreshMenu);
provider.Controller.Options.MenuItemModels.Add(saveMenu);
provider.Controller.Options.MenuItemModels.Add(advancedMenu);
```

#### Node-Level Menu

```csharp
var fileNode = TreeNodeEx.CreateNode("File");
fileNode.MenuItemModels.Add(
    new TreeNodeMenu("Open", node => OpenFile(node))
);
```

### 6. **Advanced Queries**

```csharp
var copiedNode = originalNode.Copy();

var checkedNodes = parent.GetCheckedChildren();
var allCheckedDescendants = parent.GetAllCheckedDescendants();
var hasCheckedChildren = parent.HasCheckedChildren();
```

## ⌨️ Shortcut System

### Shortcut Configuration

```csharp
var menu = new TreeViewMenu("New", CreateNewAction)
{
    Shortcut = new MenuShortcut(ModifierKeys.Control, Key.N)
};

menu.Shortcut = new MenuShortcut(ModifierKeys.Control, Key.O);
```

### Supported Shortcut Types

| Type            | Example                  | Usage              |
| --------------- | ------------------------ | ------------------ |
| Basic combo     | Ctrl+S, Ctrl+C           | Common operations  |
| Complex combo   | Ctrl+Shift+N, Alt+Ctrl+T | Advanced functions |
| Function keys   | F5, F12                  | Refresh, debug     |
| Navigation keys | Delete, Enter            | Delete, confirm    |

### Shortcut Display Rules

```csharp
// Automatically show shortcut in menu text
// "Refresh" → "Refresh (Ctrl+R)"
// "Select All" → "Select All (Ctrl+A)"
// "New Project" → "New Project (Ctrl+Shift+N)"

string displayText = menu.ShortcutDisplay;
```

### Shortcut Conflict Handling

* The system automatically handles registration conflicts.
* Later registrations override previous ones.
* Recommended strategy:

  * Common operations → simple combos (Ctrl+S, Ctrl+C)
  * Special functions → complex combos (Ctrl+Shift+*)
  * System-level → function keys (F1-F12)

## 📋 API Reference

### Node Operations

| Method                        | Description                   |
| ----------------------------- | ----------------------------- |
| `TreeNodeEx.CreateNode(text)` | Create a new node             |
| `node.AddChild(text)`         | Add child node by text        |
| `node.AddChild(childNode)`    | Add child node object         |
| `node.AddRange(nodes)`        | Add multiple child nodes      |
| `node.Copy()`                 | Copy node and subtree         |
| `node.CopyTo(parent)`         | Copy node to specified parent |
| `node.Delete()`               | Delete node                   |

### Checkbox Queries

| Method                       | Description                        |
| ---------------------------- | ---------------------------------- |
| `GetCheckedChildren()`       | Get selected direct children       |
| `GetAllCheckedDescendants()` | Get all selected descendants       |
| `GetCheckBoxChildrenCount()` | Count children with checkboxes     |
| `GetCheckedChildrenCount()`  | Count selected children            |
| `HasCheckedChildren()`       | Check if any children are selected |

### Menus & Shortcuts

| Property/Method                     | Description                   |
| ----------------------------------- | ----------------------------- |
| `Controller.Options.MenuItemModels` | Tree-level menu collection    |
| `node.MenuItemModels`               | Node-level menu collection    |
| `new TreeViewMenu(header, action)`  | Create tree-level menu        |
| `new TreeNodeMenu(header, action)`  | Create node-level menu        |
| `menu.Shortcut`                     | Set menu shortcut             |
| `menu.ShortcutDisplay`              | Get displayed shortcut text   |
| `new MenuShortcut(modifiers, key)`  | Create shortcut configuration |

## 🎯 Suitable Scenarios

### Current Version

* ✅ Small to medium datasets (< 500 nodes)
* ✅ Local data operations
* ✅ Synchronous data handling
* ✅ Basic tree display
* ✅ Full context menu support
* ✅ Checkbox support
* ✅ Shortcut key support
* ✅ Icon and style customization

### Optimized Version (Planned)

* 🔄 Large datasets (> 1000 nodes)
* 🔄 Remote data loading
* 🔄 Asynchronous operations
* 🔄 High-performance requirements
* 🔄 Complex animation effects

## Summary

**While optimizations are ongoing, the current version is already powerful!**

This project provides a **stable, fully functional, and easy-to-use** TreeView solution, covering 90% of daily development needs. For ultra-large datasets requiring maximum performance, an optimized version is under active development.

## 💡 Design Philosophy

**"Simple should not be complex; complex should not be simple."**

* Common features → plug-and-play
* Advanced features → extensible
* Misuse → compile-time errors
* Architecture → future-proof

## 🔗 Repository Links

* Gitee: [https://gitee.com/SususuChang/su.-wpf.-custom-control](https://gitee.com/SususuChang/su.-wpf.-custom-control)
* GitHub: [https://github.com/ViewSuSu/Su.WPF.TreeViewEx](https://github.com/ViewSuSu/Su.WPF.TreeViewEx)
