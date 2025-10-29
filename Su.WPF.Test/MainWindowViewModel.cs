using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Su.WPF.CustomControl.TreeViewEx;

namespace Su.WPF.Test
{
    public class MainWindowViewModel
    {
        public ImageSource Icon { get; set; }
        public ImageSource FolderIcon { get; set; }
        public ImageSource FileIcon { get; set; }
        public ImageSource ConfigIcon { get; set; }
        public ImageSource ImageIcon { get; set; }
        public ImageSource DatabaseIcon { get; set; }
        public ImageSource CodeIcon { get; set; }
        public List<TreeNodeEx> TreeNodeExs { get; set; }
        public TreeViewExProvider Provider { get; }

        public MainWindowViewModel()
        {
            // 初始化图标
            InitializeIcons();

            // 创建树节点结构
            InitializeTreeNodes();

            // 获取树视图提供者
            Provider = TreeViewExProvider.GetTreeViewPanelProvider(TreeNodeExs);

            // 配置树级别菜单
            ConfigureTreeMenus();

            // 配置节点级别菜单 - 根据节点类型配置不同的菜单
            ConfigureNodeMenusByType();

            // 验证所有节点都有菜单
            ValidateAllNodesHaveMenus();

            // 测试新增的方法
            TestNewMethods();

            // 打印初始化完成信息
            Debug.WriteLine("树控件初始化完成！");
            Debug.WriteLine($"根节点数量: {TreeNodeExs.Count}");
            Debug.WriteLine($"总节点数量: {CountAllNodes(TreeNodeExs)}");
            Debug.WriteLine($"带菜单节点数量: {CountNodesWithMenus(TreeNodeExs)}");
        }

        private void InitializeIcons()
        {
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            // 主图标
            string hamsterUri = $"pack://application:,,,/{assemblyName};component/仓鼠.png";
            Icon = new BitmapImage(new Uri(hamsterUri, UriKind.Absolute));

            // 创建不同类型的图标
            FolderIcon = CreateFolderIcon();
            FileIcon = CreateFileIcon();
            ConfigIcon = CreateConfigIcon();
            ImageIcon = CreateImageIcon();
            DatabaseIcon = CreateDatabaseIcon();
            CodeIcon = CreateCodeIcon();
        }

        private ImageSource CreateFolderIcon()
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    Brushes.Yellow,
                    new Pen(Brushes.Orange, 1),
                    new Rect(2, 4, 12, 10)
                );
                drawingContext.DrawRectangle(
                    Brushes.Yellow,
                    new Pen(Brushes.Orange, 1),
                    new Rect(4, 2, 8, 2)
                );
            }
            var renderTarget = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }

        private ImageSource CreateFileIcon()
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    Brushes.White,
                    new Pen(Brushes.Blue, 1),
                    new Rect(2, 2, 12, 12)
                );
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new Point(4, 6),
                    new Point(12, 6)
                );
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new Point(4, 9),
                    new Point(10, 9)
                );
            }
            var renderTarget = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }

        private ImageSource CreateConfigIcon()
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawEllipse(
                    Brushes.LightGreen,
                    new Pen(Brushes.Green, 1),
                    new Point(8, 8),
                    5,
                    5
                );
                drawingContext.DrawEllipse(Brushes.Green, null, new Point(8, 8), 2, 2);
            }
            var renderTarget = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }

        private ImageSource CreateImageIcon()
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    Brushes.LightPink,
                    new Pen(Brushes.Red, 1),
                    new Rect(3, 3, 10, 8)
                );
                drawingContext.DrawRectangle(Brushes.Red, null, new Rect(5, 5, 2, 2));
                drawingContext.DrawRectangle(Brushes.Red, null, new Rect(9, 5, 2, 2));
            }
            var renderTarget = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }

        private ImageSource CreateDatabaseIcon()
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawEllipse(
                    Brushes.LightBlue,
                    new Pen(Brushes.Blue, 1),
                    new Point(8, 5),
                    6,
                    3
                );
                drawingContext.DrawRectangle(
                    Brushes.LightBlue,
                    new Pen(Brushes.Blue, 1),
                    new Rect(2, 5, 12, 6)
                );
                drawingContext.DrawEllipse(
                    Brushes.LightBlue,
                    new Pen(Brushes.Blue, 1),
                    new Point(8, 11),
                    6,
                    3
                );
            }
            var renderTarget = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }

        private ImageSource CreateCodeIcon()
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawRectangle(
                    Brushes.LightGray,
                    new Pen(Brushes.DarkGray, 1),
                    new Rect(2, 2, 12, 12)
                );
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new Point(5, 5),
                    new Point(7, 7)
                );
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new Point(5, 9),
                    new Point(7, 7)
                );
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new Point(9, 5),
                    new Point(11, 7)
                );
                drawingContext.DrawLine(
                    new Pen(Brushes.Black, 1),
                    new Point(9, 9),
                    new Point(11, 7)
                );
            }
            var renderTarget = new RenderTargetBitmap(16, 16, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(drawingVisual);
            return renderTarget;
        }

        private void InitializeTreeNodes()
        {
            TreeNodeExs = new List<TreeNodeEx>();

            // === 不同类型的根节点 ===
            var projectRoot = CreateProjectStructure(); // 项目结构 - 完整功能
            var documentRoot = CreateDocumentStructure(); // 文档结构 - 只读操作
            var settingsRoot = CreateSettingsStructure(); // 设置结构 - 配置操作
            var databaseRoot = CreateDatabaseStructure(); // 数据库结构 - 数据操作
            var emptyRoot = CreateEmptyStructure(); // 空结构 - 测试操作

            TreeNodeExs.Add(projectRoot);
            TreeNodeExs.Add(documentRoot);
            TreeNodeExs.Add(settingsRoot);
            TreeNodeExs.Add(databaseRoot);
            TreeNodeExs.Add(emptyRoot);
        }

        private TreeNodeEx CreateProjectStructure()
        {
            var root = CreateNode(
                "项目解决方案",
                FolderIcon,
                true,
                Colors.LightBlue,
                "完整的项目解决方案，包含所有源代码和资源"
            );
            root.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;

            // 核心代码 - 不允许删除
            var coreFolder = CreateNode(
                "核心代码",
                CodeIcon,
                true,
                Colors.LightBlue,
                "核心业务逻辑代码"
            );
            coreFolder.IsEnabled = false; // 核心代码不允许修改

            var modelsFolder = CreateNode(
                "数据模型",
                CodeIcon,
                true,
                Colors.Transparent,
                "数据模型定义"
            );
            var servicesFolder = CreateNode(
                "服务层",
                CodeIcon,
                true,
                Colors.Transparent,
                "业务服务实现"
            );

            // 设置一些节点为选中状态，用于测试新方法
            modelsFolder.IsChecked = true;
            servicesFolder.IsChecked = false;

            coreFolder.AddRange(new[] { modelsFolder, servicesFolder });

            // UI项目 - 完整操作权限
            var uiProject = CreateNode(
                "UI项目",
                FolderIcon,
                true,
                Colors.LightGreen,
                "用户界面项目"
            );
            var viewsFolder = CreateNode(
                "视图",
                FolderIcon,
                true,
                Colors.Transparent,
                "用户界面视图"
            );
            var viewModelsFolder = CreateNode(
                "视图模型",
                CodeIcon,
                true,
                Colors.Transparent,
                "MVVM视图模型"
            );

            // 设置选中状态
            viewsFolder.IsChecked = true;
            viewModelsFolder.IsChecked = true;

            uiProject.AddRange(new[] { viewsFolder, viewModelsFolder });

            // 测试项目 - 测试相关操作
            var testProject = CreateNode(
                "测试项目",
                FolderIcon,
                true,
                Colors.LightYellow,
                "单元测试和集成测试"
            );
            var unitTestsFolder = CreateNode(
                "单元测试",
                CodeIcon,
                true,
                Colors.Transparent,
                "单元测试代码"
            );
            var integrationTestsFolder = CreateNode(
                "集成测试",
                CodeIcon,
                true,
                Colors.Transparent,
                "集成测试代码"
            );

            // 设置选中状态
            unitTestsFolder.IsChecked = false;
            integrationTestsFolder.IsChecked = true;

            testProject.AddRange(new[] { unitTestsFolder, integrationTestsFolder });

            root.AddRange(new[] { coreFolder, uiProject, testProject });
            return root;
        }

        private TreeNodeEx CreateDocumentStructure()
        {
            var root = CreateNode(
                "项目文档",
                FolderIcon,
                false,
                Colors.LightGreen,
                "项目相关文档和规范"
            );
            root.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;
            root.IsExpanded = true;

            // 需求文档 - 只读
            var requirementsFolder = CreateNode(
                "需求文档",
                FolderIcon,
                false,
                Colors.LightGreen,
                "项目需求文档"
            );
            var prdDoc = CreateNode(
                "产品需求文档.docx",
                FileIcon,
                false,
                Colors.Transparent,
                "产品需求说明"
            );
            var userStoriesDoc = CreateNode(
                "用户故事.md",
                FileIcon,
                false,
                Colors.Transparent,
                "用户故事描述"
            );

            requirementsFolder.AddRange(new[] { prdDoc, userStoriesDoc });

            // 设计文档 - 只读
            var designFolder = CreateNode(
                "设计文档",
                FolderIcon,
                false,
                Colors.LightGreen,
                "系统设计文档"
            );
            var architectureDoc = CreateNode(
                "架构设计.pdf",
                FileIcon,
                false,
                Colors.Transparent,
                "系统架构设计"
            );
            var apiDoc = CreateNode(
                "API文档.md",
                FileIcon,
                false,
                Colors.Transparent,
                "API接口文档"
            );

            designFolder.AddRange(new[] { architectureDoc, apiDoc });

            // 开发规范 - 可修改
            var standardsFolder = CreateNode(
                "开发规范",
                FolderIcon,
                true,
                Colors.LightGreen,
                "代码规范和开发指南"
            );
            var codingStandards = CreateNode(
                "编码规范.md",
                FileIcon,
                true,
                Colors.Transparent,
                "代码编写规范"
            );
            var gitStandards = CreateNode(
                "Git规范.md",
                FileIcon,
                true,
                Colors.Transparent,
                "版本控制规范"
            );

            // 设置选中状态
            codingStandards.IsChecked = true;
            gitStandards.IsChecked = false;

            standardsFolder.AddRange(new[] { codingStandards, gitStandards });

            root.AddRange(new[] { requirementsFolder, designFolder, standardsFolder });
            return root;
        }

        private TreeNodeEx CreateSettingsStructure()
        {
            var root = CreateNode(
                "系统配置",
                ConfigIcon,
                true,
                Colors.LightYellow,
                "应用程序配置和设置"
            );
            root.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;

            // 用户配置 - 完整操作
            var userConfig = CreateNode(
                "用户配置",
                ConfigIcon,
                true,
                Colors.LightYellow,
                "用户个性化设置"
            );
            var appearanceSettings = CreateNode(
                "外观设置",
                ConfigIcon,
                true,
                Colors.Transparent,
                "界面外观配置"
            );
            var behaviorSettings = CreateNode(
                "行为设置",
                ConfigIcon,
                true,
                Colors.Transparent,
                "应用程序行为配置"
            );

            // 设置选中状态
            appearanceSettings.IsChecked = true;
            behaviorSettings.IsChecked = true;

            userConfig.AddRange(new[] { appearanceSettings, behaviorSettings });

            // 系统配置 - 受限操作
            var systemConfig = CreateNode(
                "系统配置",
                ConfigIcon,
                false,
                Colors.LightGray,
                "系统级配置（只读）"
            );
            var networkSettings = CreateNode(
                "网络配置",
                ConfigIcon,
                false,
                Colors.Transparent,
                "网络连接设置"
            );
            var securitySettings = CreateNode(
                "安全配置",
                ConfigIcon,
                false,
                Colors.Transparent,
                "安全相关设置"
            );

            systemConfig.AddRange(new[] { networkSettings, securitySettings });

            // 插件配置 - 动态操作
            var pluginsConfig = CreateNode(
                "插件管理",
                ConfigIcon,
                true,
                Colors.LightCoral,
                "插件安装和配置"
            );
            var installedPlugins = CreateNode(
                "已安装插件",
                ConfigIcon,
                true,
                Colors.Transparent,
                "已安装的插件列表"
            );
            var availablePlugins = CreateNode(
                "可用插件",
                ConfigIcon,
                true,
                Colors.Transparent,
                "可安装的插件"
            );

            // 设置选中状态
            installedPlugins.IsChecked = true;
            availablePlugins.IsChecked = false;

            pluginsConfig.AddRange(new[] { installedPlugins, availablePlugins });

            root.AddRange(new[] { userConfig, systemConfig, pluginsConfig });
            return root;
        }

        private TreeNodeEx CreateDatabaseStructure()
        {
            var root = CreateNode(
                "数据库管理",
                DatabaseIcon,
                true,
                Colors.Aqua,
                "数据库表和数据管理"
            );
            root.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;

            // 系统表 - 只读
            var systemTables = CreateNode(
                "系统表",
                DatabaseIcon,
                false,
                Colors.LightGray,
                "数据库系统表（只读）"
            );
            var usersTable = CreateNode("Users", DatabaseIcon, false, Colors.Transparent, "用户表");
            var rolesTable = CreateNode("Roles", DatabaseIcon, false, Colors.Transparent, "角色表");

            systemTables.AddRange(new[] { usersTable, rolesTable });

            // 业务表 - 完整操作
            var businessTables = CreateNode(
                "业务表",
                DatabaseIcon,
                true,
                Colors.Aqua,
                "业务数据表"
            );
            var productsTable = CreateNode(
                "Products",
                DatabaseIcon,
                true,
                Colors.Transparent,
                "产品表"
            );
            var ordersTable = CreateNode(
                "Orders",
                DatabaseIcon,
                true,
                Colors.Transparent,
                "订单表"
            );
            var customersTable = CreateNode(
                "Customers",
                DatabaseIcon,
                true,
                Colors.Transparent,
                "客户表"
            );

            // 设置选中状态
            productsTable.IsChecked = true;
            ordersTable.IsChecked = false;
            customersTable.IsChecked = true;

            businessTables.AddRange(new[] { productsTable, ordersTable, customersTable });

            // 视图和存储过程
            var databaseObjects = CreateNode(
                "数据库对象",
                DatabaseIcon,
                true,
                Colors.LightBlue,
                "视图、存储过程等"
            );
            var views = CreateNode("视图", DatabaseIcon, true, Colors.Transparent, "数据库视图");
            var storedProcedures = CreateNode(
                "存储过程",
                DatabaseIcon,
                true,
                Colors.Transparent,
                "存储过程"
            );

            // 设置选中状态
            views.IsChecked = true;
            storedProcedures.IsChecked = false;

            databaseObjects.AddRange(new[] { views, storedProcedures });

            root.AddRange(new[] { systemTables, businessTables, databaseObjects });
            return root;
        }

        private TreeNodeEx CreateEmptyStructure()
        {
            var root = CreateNode(
                "测试和开发",
                FolderIcon,
                true,
                Colors.LightCoral,
                "用于测试和开发的节点"
            );
            root.TreeNodeExTextOptions.FontWeight = FontWeights.Bold;

            // 空文件夹 - 测试添加功能
            var emptyFolder = CreateNode(
                "空文件夹",
                FolderIcon,
                true,
                Colors.LightCoral,
                "没有任何内容的文件夹，测试添加功能"
            );

            // 单文件节点 - 测试文件操作
            var singleFile = CreateNode(
                "示例文件.txt",
                FileIcon,
                true,
                Colors.Transparent,
                "独立的文件节点，测试文件操作"
            );

            // 禁用节点 - 测试禁用状态
            var disabledNode = CreateNode(
                "禁用节点",
                FolderIcon,
                true,
                Colors.LightGray,
                "被禁用的节点，测试禁用状态"
            );
            disabledNode.IsEnabled = false;

            // 隐藏节点 - 测试动态显示
            var hiddenNode = CreateNode(
                "动态显示节点",
                FolderIcon,
                true,
                Colors.LightPink,
                "根据条件显示的节点"
            );

            // 设置选中状态
            singleFile.IsChecked = true;
            hiddenNode.IsChecked = false;

            root.AddRange(new[] { emptyFolder, singleFile, disabledNode, hiddenNode });
            return root;
        }

        private TreeNodeEx CreateNode(
            string text,
            ImageSource icon,
            bool showCheckBox,
            Color highlightColor,
            string tooltip = ""
        )
        {
            var node = TreeNodeEx.CreateNode(text);
            node.TreeNodeExIconOptions.Icon = icon;
            node.IsShowCheckBox = showCheckBox;
            node.HighlightColor = highlightColor;
            node.Tooltip = tooltip;
            return node;
        }

        private void ConfigureTreeMenus()
        {
            // 全局操作菜单
            var globalMenus = new[]
            {
                CreateTreeViewMenu(
                    "刷新",
                    RefreshTree,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.R
                ),
                CreateTreeViewMenu(
                    "全部展开",
                    ExpandAllNodes,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.E
                ),
                CreateTreeViewMenu(
                    "全部折叠",
                    CollapseAllNodes,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.W
                ),
            };

            // 管理操作菜单
            var managementMenus = new[]
            {
                CreateTreeViewMenu(
                    "添加根项目",
                    AddRootProject,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.N
                ),
                CreateTreeViewMenu(
                    "导出结构",
                    ExportStructure,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.S
                ),
                CreateTreeViewMenu(
                    "导入结构",
                    ImportStructure,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.O
                ),
                CreateTreeViewMenu(
                    "测试新增方法",
                    TestNewMethods,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.T
                ),
            };

            // 查看和统计菜单
            var infoMenus = new[]
            {
                CreateTreeViewMenu(
                    "显示选中项",
                    ShowSelectedNodes,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.D
                ),
                CreateTreeViewMenu(
                    "统计信息",
                    CountNodesInfo,
                    System.Windows.Input.ModifierKeys.Control,
                    System.Windows.Input.Key.I
                ),
                CreateTreeViewMenu("验证完整性", ValidateAllNodesHaveMenus, null, null),
                CreateTreeViewMenu("测试复选框方法", TestCheckBoxMethods, null, null),
                CreateTreeViewMenu("测试复制方法", TestCopyMethods, null, null),
            };

            foreach (var menu in globalMenus.Concat(managementMenus).Concat(infoMenus))
            {
                Provider.Controller.Options.MenuItemModels.Add(menu);
            }
        }

        private void ConfigureNodeMenusByType()
        {
            // 为所有节点配置基于类型的菜单
            foreach (var rootNode in TreeNodeExs)
            {
                ConfigureNodeMenusRecursive(rootNode);
            }

            Debug.WriteLine("基于节点类型的菜单配置完成");
        }

        private void ConfigureNodeMenusRecursive(TreeNodeEx node)
        {
            // 清除现有菜单
            node.MenuItemModels.Clear();

            // 根据节点特征配置不同的菜单
            if (node.Text.Contains("核心代码") || node.Text.Contains("系统表"))
            {
                // 核心节点 - 只读操作
                AddCoreNodeMenus(node);
            }
            else if (node.Text.Contains("文档") || !node.IsEnabled)
            {
                // 文档节点或禁用节点 - 受限操作
                AddDocumentNodeMenus(node);
            }
            else if (node.Text.Contains("配置") || node.Text.Contains("设置"))
            {
                // 配置节点 - 配置相关操作
                AddConfigNodeMenus(node);
            }
            else if (node.Text.Contains("数据库") || node.Text.Contains("Table"))
            {
                // 数据库节点 - 数据操作
                AddDatabaseNodeMenus(node);
            }
            else if (node.Children.Count == 0)
            {
                // 叶子节点 - 文件操作
                AddLeafNodeMenus(node);
            }
            else
            {
                // 普通文件夹节点 - 完整操作
                AddFolderNodeMenus(node);
            }

            // 递归配置子节点
            foreach (var child in node.Children)
            {
                ConfigureNodeMenusRecursive(child);
            }
        }

        private void AddCoreNodeMenus(TreeNodeEx node)
        {
            var menus = new[]
            {
                new CustomControl.Menu.TreeNodeMenu("查看信息", ShowNodeInfo),
                new CustomControl.Menu.TreeNodeMenu(
                    "复制路径",
                    n => Debug.WriteLine($"路径: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "导出代码",
                    n => Debug.WriteLine($"导出: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu("测试复制方法", TestCopyNode),
            };
            AddMenusToNode(node, menus);
        }

        private void AddDocumentNodeMenus(TreeNodeEx node)
        {
            var menus = new[]
            {
                new CustomControl.Menu.TreeNodeMenu(
                    "打开文档",
                    n => Debug.WriteLine($"打开: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu("查看信息", ShowNodeInfo),
                new CustomControl.Menu.TreeNodeMenu(
                    "复制名称",
                    n => Debug.WriteLine($"复制: {n.Text}")
                ),
            };

            if (node.IsEnabled)
            {
                var editableMenus = new[]
                {
                    new CustomControl.Menu.TreeNodeMenu(
                        "编辑文档",
                        n => Debug.WriteLine($"编辑: {n.Text}")
                    ),
                    new CustomControl.Menu.TreeNodeMenu(
                        "版本历史",
                        n => Debug.WriteLine($"历史: {n.Text}")
                    ),
                    new CustomControl.Menu.TreeNodeMenu("测试复制方法", TestCopyNode),
                };
                menus = menus.Concat(editableMenus).ToArray();
            }

            AddMenusToNode(node, menus);
        }

        private void AddConfigNodeMenus(TreeNodeEx node)
        {
            var menus = new[]
            {
                new CustomControl.Menu.TreeNodeMenu(
                    "编辑配置",
                    n => Debug.WriteLine($"编辑配置: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "重置默认",
                    n => Debug.WriteLine($"重置: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "导入配置",
                    n => Debug.WriteLine($"导入: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "导出配置",
                    n => Debug.WriteLine($"导出: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu("查看信息", ShowNodeInfo),
                new CustomControl.Menu.TreeNodeMenu("测试复制方法", TestCopyNode),
                new CustomControl.Menu.TreeNodeMenu("测试复选框方法", TestNodeCheckBoxMethods),
            };
            AddMenusToNode(node, menus);
        }

        private void AddDatabaseNodeMenus(TreeNodeEx node)
        {
            var menus = new[]
            {
                new CustomControl.Menu.TreeNodeMenu(
                    "查询数据",
                    n => Debug.WriteLine($"查询: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "设计表结构",
                    n => Debug.WriteLine($"设计: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "备份数据",
                    n => Debug.WriteLine($"备份: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu("查看信息", ShowNodeInfo),
                new CustomControl.Menu.TreeNodeMenu("测试复制方法", TestCopyNode),
                new CustomControl.Menu.TreeNodeMenu("测试复选框方法", TestNodeCheckBoxMethods),
            };

            if (!node.Text.Contains("系统表"))
            {
                var managementMenus = new[]
                {
                    new CustomControl.Menu.TreeNodeMenu(
                        "清空数据",
                        n => Debug.WriteLine($"清空: {n.Text}")
                    ),
                    new CustomControl.Menu.TreeNodeMenu(
                        "优化表",
                        n => Debug.WriteLine($"优化: {n.Text}")
                    ),
                };
                menus = menus.Concat(managementMenus).ToArray();
            }

            AddMenusToNode(node, menus);
        }

        private void AddFolderNodeMenus(TreeNodeEx node)
        {
            var menus = new[]
            {
                new CustomControl.Menu.TreeNodeMenu("新建文件", AddChildNodeWithMenu),
                new CustomControl.Menu.TreeNodeMenu(
                    "新建文件夹",
                    n =>
                    {
                        var folder = CreateNode(
                            "新建文件夹",
                            FolderIcon,
                            true,
                            Colors.LightGray,
                            "新建的文件夹"
                        );
                        ConfigureNodeMenusRecursive(folder);
                        n.AddChild(folder);
                        n.IsExpanded = true;
                    }
                ),
                new CustomControl.Menu.TreeNodeMenu("重命名", RenameNode),
                new CustomControl.Menu.TreeNodeMenu("删除", n => n.Delete()),
                new CustomControl.Menu.TreeNodeMenu("查看信息", ShowNodeInfo),
                new CustomControl.Menu.TreeNodeMenu(
                    "展开全部",
                    n => SetAllNodesExpandedState(new[] { n }, true)
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "折叠全部",
                    n => SetAllNodesExpandedState(new[] { n }, false)
                ),
                new CustomControl.Menu.TreeNodeMenu("测试复制方法", TestCopyNode),
                new CustomControl.Menu.TreeNodeMenu("测试复选框方法", TestNodeCheckBoxMethods),
            };
            AddMenusToNode(node, menus);
        }

        private void AddLeafNodeMenus(TreeNodeEx node)
        {
            var menus = new[]
            {
                new CustomControl.Menu.TreeNodeMenu(
                    "打开文件",
                    n => Debug.WriteLine($"打开: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu(
                    "编辑内容",
                    n => Debug.WriteLine($"编辑: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu("重命名", RenameNode),
                new CustomControl.Menu.TreeNodeMenu("删除", n => n.Delete()),
                new CustomControl.Menu.TreeNodeMenu("测试复制方法", TestCopyNode),
                new CustomControl.Menu.TreeNodeMenu("查看信息", ShowNodeInfo),
                new CustomControl.Menu.TreeNodeMenu(
                    "属性",
                    n => Debug.WriteLine($"属性: {n.Text}")
                ),
                new CustomControl.Menu.TreeNodeMenu("测试复选框方法", TestNodeCheckBoxMethods),
            };
            AddMenusToNode(node, menus);
        }

        private void AddMenusToNode(TreeNodeEx node, CustomControl.Menu.TreeNodeMenu[] menus)
        {
            foreach (var menu in menus)
            {
                node.MenuItemModels.Add(menu);
            }
        }

        #region 新增方法的测试

        private void TestNewMethods()
        {
            Debug.WriteLine("=== 开始测试新增方法 ===");

            // 测试复制方法
            TestCopyMethods();

            // 测试复选框相关方法
            TestCheckBoxMethods();

            Debug.WriteLine("=== 新增方法测试完成 ===");
        }

        private void TestCopyMethods()
        {
            Debug.WriteLine("\n--- 测试复制方法 ---");

            // 测试基本复制
            var originalNode = TreeNodeExs[0]; // 项目解决方案
            var copiedNode = originalNode.Copy();
            Debug.WriteLine($"基本复制: {originalNode.Text} -> {copiedNode.Text}");

            // 测试 CopyTo 方法 - 复制到指定位置
            var targetParent = TreeNodeExs[4]; // 测试和开发节点
            var originalChildCount = targetParent.Children.Count;
            var copiedToNode = originalNode.CopyTo(targetParent);
            Debug.WriteLine($"CopyTo 方法: {copiedToNode.Text} 已添加到 {targetParent.Text}");

            // 验证复制结果
            Debug.WriteLine($"原始节点子节点数: {originalNode.Children.Count}");
            Debug.WriteLine($"基本复制节点子节点数: {copiedNode.Children.Count}");
            Debug.WriteLine($"CopyTo 复制节点子节点数: {copiedToNode.Children.Count}");

            // 验证 CopyTo 确实添加到了目标父节点
            Debug.WriteLine($"目标父节点原来有 {originalChildCount} 个子节点");
            Debug.WriteLine($"目标父节点现在有 {targetParent.Children.Count} 个子节点");
            Debug.WriteLine($"最后一个子节点是: {targetParent.Children.Last().Text}");

            // 验证复制的内容完整性
            Debug.WriteLine($"原始节点菜单数: {originalNode.MenuItems.Count}");
            Debug.WriteLine($"复制节点菜单数: {copiedNode.MenuItems.Count}");
            Debug.WriteLine($"CopyTo节点菜单数: {copiedToNode.MenuItems.Count}");
        }

        private void TestCheckBoxMethods()
        {
            Debug.WriteLine("\n--- 测试复选框相关方法 ---");

            foreach (var rootNode in TreeNodeExs)
            {
                TestNodeCheckBoxMethods(rootNode);
            }
        }

        private void TestNodeCheckBoxMethods(TreeNodeEx node)
        {
            Debug.WriteLine($"\n测试节点: {node.Text}");

            // 测试 GetCheckedChildren()
            var checkedChildren = node.GetCheckedChildren();
            Debug.WriteLine($"  GetCheckedChildren(): {checkedChildren.Count} 个选中子节点");
            foreach (var child in checkedChildren)
            {
                Debug.WriteLine($"    - {child.Text}");
            }

            // 测试 GetAllCheckedDescendants()
            var allCheckedDescendants = node.GetAllCheckedDescendants();
            Debug.WriteLine(
                $"  GetAllCheckedDescendants(): {allCheckedDescendants.Count} 个选中子孙节点"
            );

            // 测试 GetCheckBoxChildrenCount()
            var checkboxCount = node.GetCheckBoxChildrenCount();
            Debug.WriteLine($"  GetCheckBoxChildrenCount(): {checkboxCount} 个显示复选框的子节点");

            // 测试 GetCheckedChildrenCount()
            var checkedCount = node.GetCheckedChildrenCount();
            Debug.WriteLine($"  GetCheckedChildrenCount(): {checkedCount} 个选中的子节点");

            // 测试 HasCheckedChildren()
            var hasChecked = node.HasCheckedChildren();
            Debug.WriteLine($"  HasCheckedChildren(): {hasChecked}");

            // 递归测试子节点
            foreach (var child in node.Children)
            {
                TestNodeCheckBoxMethods(child);
            }
        }

        private void TestCopyNode(TreeNodeEx node)
        {
            Debug.WriteLine($"\n--- 测试节点复制: {node.Text} ---");

            // 测试基本 Copy 方法
            var copiedNode = node.Copy();
            Debug.WriteLine($"Copy() 方法: {node.Text} -> {copiedNode.Text}");

            // 测试 CopyTo 方法
            var tempParent = CreateNode(
                "临时父节点",
                FolderIcon,
                true,
                Colors.LightGray,
                "用于测试 CopyTo"
            );
            var originalTempChildCount = tempParent.Children.Count;
            var copiedToNode = node.CopyTo(tempParent);
            Debug.WriteLine(
                $"CopyTo() 方法: {node.Text} -> {copiedToNode.Text} (添加到 {tempParent.Text})"
            );

            // 验证复制内容
            Debug.WriteLine($"原始节点菜单数: {node.MenuItems.Count}");
            Debug.WriteLine($"Copy() 复制节点菜单数: {copiedNode.MenuItems.Count}");
            Debug.WriteLine($"CopyTo() 复制节点菜单数: {copiedToNode.MenuItems.Count}");
            Debug.WriteLine($"原始节点子节点数: {node.Children.Count}");
            Debug.WriteLine($"Copy() 复制节点子节点数: {copiedNode.Children.Count}");
            Debug.WriteLine($"CopyTo() 复制节点子节点数: {copiedToNode.Children.Count}");
            Debug.WriteLine($"临时父节点原来有 {originalTempChildCount} 个子节点");
            Debug.WriteLine($"临时父节点现在有 {tempParent.Children.Count} 个子节点");

            // 验证 CopyTo 是否真的添加了节点
            if (tempParent.Children.Contains(copiedToNode))
            {
                Debug.WriteLine("✅ CopyTo 方法验证成功：节点已正确添加到目标父节点");
            }
            else
            {
                Debug.WriteLine("❌ CopyTo 方法验证失败：节点未添加到目标父节点");
            }
        }

        #endregion

        #region 原有的辅助方法和菜单实现

        private CustomControl.Menu.TreeViewMenu CreateTreeViewMenu(
            string header,
            Action action,
            System.Windows.Input.ModifierKeys? modifiers,
            System.Windows.Input.Key? key
        )
        {
            var menu = new CustomControl.Menu.TreeViewMenu(header, action) { Icon = Icon };
            if (modifiers.HasValue && key.HasValue)
            {
                menu.Shortcut = new CustomControl.Menu.MenuShortcut(modifiers.Value, key.Value);
            }
            return menu;
        }

        private void ValidateAllNodesHaveMenus()
        {
            int totalNodes = CountAllNodes(TreeNodeExs);
            int nodesWithMenus = CountNodesWithMenus(TreeNodeExs);
            Debug.WriteLine($"菜单验证: {nodesWithMenus}/{totalNodes} 节点有菜单");

            if (nodesWithMenus == totalNodes)
            {
                Debug.WriteLine("✅ 所有节点都已配置右键菜单！");
            }
            else
            {
                Debug.WriteLine("❌ 存在未配置菜单的节点！");
                FindNodesWithoutMenus(TreeNodeExs);
            }
        }

        private void FindNodesWithoutMenus(IEnumerable<TreeNodeEx> nodes, string path = "")
        {
            foreach (var node in nodes)
            {
                string currentPath = string.IsNullOrEmpty(path) ? node.Text : $"{path}/{node.Text}";

                if (node.MenuItems.Count == 0)
                {
                    Debug.WriteLine($"  无菜单节点: {currentPath}");
                }

                FindNodesWithoutMenus(node.Children, currentPath);
            }
        }

        private int CountAllNodes(IEnumerable<TreeNodeEx> nodes)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                count++;
                count += CountAllNodes(node.Children);
            }
            return count;
        }

        private int CountNodesWithMenus(IEnumerable<TreeNodeEx> nodes)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                if (node.MenuItems.Count > 0)
                    count++;
                count += CountNodesWithMenus(node.Children);
            }
            return count;
        }

        // 原有的菜单命令实现方法
        private void RefreshTree() => Debug.WriteLine("刷新树结构");

        private void ExpandAllNodes() => SetAllNodesExpandedState(TreeNodeExs, true);

        private void CollapseAllNodes() => SetAllNodesExpandedState(TreeNodeExs, false);

        private void AddRootProject()
        {
            var newNode = CreateNode(
                $"新项目_{DateTime.Now:HHmmss}",
                FolderIcon,
                true,
                Colors.LightGray,
                "新添加的根项目"
            );
            ConfigureNodeMenusRecursive(newNode);
            Provider.Controller.SourceTreeNodes.Add(newNode);
            Debug.WriteLine($"添加根项目: {newNode.Text}");
        }

        private void ExportStructure() => Debug.WriteLine("导出结构");

        private void ImportStructure() => Debug.WriteLine("导入结构");

        private void ShowSelectedNodes() =>
            Debug.WriteLine($"选中 {Provider.Controller.SelectedNodes.Count} 个节点");

        private void CountNodesInfo()
        {
            Debug.WriteLine($"总节点数: {CountAllNodes(TreeNodeExs)}");
            Debug.WriteLine($"根节点数: {TreeNodeExs.Count}");
            Debug.WriteLine($"带复选框节点数: {CountNodesWithCheckBox(TreeNodeExs)}");
            Debug.WriteLine($"选中节点数: {CountCheckedNodes(TreeNodeExs)}");
        }

        private int CountNodesWithCheckBox(IEnumerable<TreeNodeEx> nodes)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                if (node.IsShowCheckBox)
                    count++;
                count += CountNodesWithCheckBox(node.Children);
            }
            return count;
        }

        private int CountCheckedNodes(IEnumerable<TreeNodeEx> nodes)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                if (node.IsShowCheckBox && node.IsChecked == true)
                    count++;
                count += CountCheckedNodes(node.Children);
            }
            return count;
        }

        private void SetAllNodesExpandedState(IEnumerable<TreeNodeEx> nodes, bool isExpanded)
        {
            foreach (var node in nodes)
            {
                node.IsExpanded = isExpanded;
                SetAllNodesExpandedState(node.Children, isExpanded);
            }
        }

        private void AddChildNodeWithMenu(TreeNodeEx node)
        {
            var childNode = CreateNode(
                $"新文件_{DateTime.Now:HHmmss}.txt",
                FileIcon,
                true,
                Colors.Transparent,
                "新创建的文件"
            );
            ConfigureNodeMenusRecursive(childNode);
            node.AddChild(childNode);
            node.IsExpanded = true;
            Debug.WriteLine($"为 {node.Text} 添加子节点: {childNode.Text}");
        }

        private void RenameNode(TreeNodeEx node)
        {
            node.Text = $"{node.Text}_重命名_{DateTime.Now:HHmmss}";
            Debug.WriteLine($"重命名: {node.Text}");
        }

        private void ShowNodeInfo(TreeNodeEx node)
        {
            Debug.WriteLine($"=== 节点详细信息: {node.Text} ===");
            Debug.WriteLine($"菜单数: {node.MenuItems.Count}");
            Debug.WriteLine($"子节点数: {node.Children.Count}");
            Debug.WriteLine($"选中状态: {node.IsChecked}");
            Debug.WriteLine($"启用状态: {node.IsEnabled}");
            Debug.WriteLine($"显示复选框: {node.IsShowCheckBox}");
            Debug.WriteLine($"展开状态: {node.IsExpanded}");
            Debug.WriteLine($"工具提示: {node.Tooltip}");
            Debug.WriteLine($"父节点: {node.FatherNode?.Text ?? "根节点"}");
        }

        #endregion
    }
}
