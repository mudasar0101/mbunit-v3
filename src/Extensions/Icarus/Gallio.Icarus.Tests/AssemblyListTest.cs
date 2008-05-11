// Copyright 2005-2008 Gallio Project - http://www.gallio.org/
// Portions Copyright 2000-2004 Jonathan de Halleux
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Gallio.Icarus.Interfaces;

using MbUnit.Framework;

namespace Gallio.Icarus.Tests
{
    [TestFixture]
    public class AssemblyListTest : MockTest
    {
        private AssemblyList assemblyList;

        [SetUp]
        public void SetUp()
        {
            IProjectAdapterView projectAdapterView = mocks.CreateMock<IProjectAdapterView>();
            mocks.ReplayAll();
            assemblyList = new AssemblyList(projectAdapterView);
        }

        [Test]
        public void DataBind_Test()
        {
            ListViewItem[] listViewItems = new ListViewItem[0];
            assemblyList.DataBind(listViewItems);
        }
    }
}
