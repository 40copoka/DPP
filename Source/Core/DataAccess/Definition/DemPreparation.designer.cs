﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MilSpace.DataAccess.Definition
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="DemPreparation")]
	public partial class DemPreparationContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertS1TilesCoverage(S1TilesCoverage instance);
    partial void UpdateS1TilesCoverage(S1TilesCoverage instance);
    partial void DeleteS1TilesCoverage(S1TilesCoverage instance);
    partial void InsertS1PairCoherence(S1PairCoherence instance);
    partial void UpdateS1PairCoherence(S1PairCoherence instance);
    partial void DeleteS1PairCoherence(S1PairCoherence instance);
    partial void InsertS1Sources(S1Sources instance);
    partial void UpdateS1Sources(S1Sources instance);
    partial void DeleteS1Sources(S1Sources instance);
    #endregion
		
		public DemPreparationContext() : 
				base(global::MilSpace.DataAccess.Properties.Settings.Default.DemPreparationConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public DemPreparationContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DemPreparationContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DemPreparationContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DemPreparationContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		internal System.Data.Linq.Table<S1TilesCoverage> S1TilesCoverages
		{
			get
			{
				return this.GetTable<S1TilesCoverage>();
			}
		}
		
		internal System.Data.Linq.Table<S1PairCoherence> S1PairCoherences
		{
			get
			{
				return this.GetTable<S1PairCoherence>();
			}
		}
		
		internal System.Data.Linq.Table<S1Sources> S1Sources
		{
			get
			{
				return this.GetTable<S1Sources>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MilSpace_S1TilesCover")]
	internal partial class S1TilesCoverage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idrow;
		
		private string _DegreeTileName;
		
		private System.Nullable<System.DateTime> _DTBaseSurvey;
		
		private System.Nullable<int> _idPair1;
		
		private System.Nullable<int> _idPair2;
		
		private System.Nullable<int> _Pair1IW1;
		
		private System.Nullable<int> _Pair1IW1B1;
		
		private System.Nullable<int> _Pair1IW1B2;
		
		private System.Nullable<int> _Pair1IW2;
		
		private System.Nullable<int> _Pair1IW2B1;
		
		private System.Nullable<int> _Pair1IW2B2;
		
		private System.Nullable<int> _Pair1IW3;
		
		private System.Nullable<int> _Pair1IW3B1;
		
		private System.Nullable<int> _Pair1IW3B2;
		
		private System.Nullable<int> _Pair2IW1;
		
		private System.Nullable<int> _Pair2IW1B1;
		
		private System.Nullable<int> _Pair2IW1B2;
		
		private System.Nullable<int> _Pair2IW2;
		
		private System.Nullable<int> _Pair2IW2B1;
		
		private System.Nullable<int> _Pair2IW2B2;
		
		private System.Nullable<int> _Pair2IW3;
		
		private System.Nullable<int> _Pair2IW3B1;
		
		private System.Nullable<int> _Pair2IW3B2;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidrowChanging(int value);
    partial void OnidrowChanged();
    partial void OnDegreeTileNameChanging(string value);
    partial void OnDegreeTileNameChanged();
    partial void OnDTBaseSurveyChanging(System.Nullable<System.DateTime> value);
    partial void OnDTBaseSurveyChanged();
    partial void OnidPair1Changing(System.Nullable<int> value);
    partial void OnidPair1Changed();
    partial void OnidPair2Changing(System.Nullable<int> value);
    partial void OnidPair2Changed();
    partial void OnPair1IW1Changing(System.Nullable<int> value);
    partial void OnPair1IW1Changed();
    partial void OnPair1IW1B1Changing(System.Nullable<int> value);
    partial void OnPair1IW1B1Changed();
    partial void OnPair1IW1B2Changing(System.Nullable<int> value);
    partial void OnPair1IW1B2Changed();
    partial void OnPair1IW2Changing(System.Nullable<int> value);
    partial void OnPair1IW2Changed();
    partial void OnPair1IW2B1Changing(System.Nullable<int> value);
    partial void OnPair1IW2B1Changed();
    partial void OnPair1IW2B2Changing(System.Nullable<int> value);
    partial void OnPair1IW2B2Changed();
    partial void OnPair1IW3Changing(System.Nullable<int> value);
    partial void OnPair1IW3Changed();
    partial void OnPair1IW3B1Changing(System.Nullable<int> value);
    partial void OnPair1IW3B1Changed();
    partial void OnPair1IW3B2Changing(System.Nullable<int> value);
    partial void OnPair1IW3B2Changed();
    partial void OnPair2IW1Changing(System.Nullable<int> value);
    partial void OnPair2IW1Changed();
    partial void OnPair2IW1B1Changing(System.Nullable<int> value);
    partial void OnPair2IW1B1Changed();
    partial void OnPair2IW1B2Changing(System.Nullable<int> value);
    partial void OnPair2IW1B2Changed();
    partial void OnPair2IW2Changing(System.Nullable<int> value);
    partial void OnPair2IW2Changed();
    partial void OnPair2IW2B1Changing(System.Nullable<int> value);
    partial void OnPair2IW2B1Changed();
    partial void OnPair2IW2B2Changing(System.Nullable<int> value);
    partial void OnPair2IW2B2Changed();
    partial void OnPair2IW3Changing(System.Nullable<int> value);
    partial void OnPair2IW3Changed();
    partial void OnPair2IW3B1Changing(System.Nullable<int> value);
    partial void OnPair2IW3B1Changed();
    partial void OnPair2IW3B2Changing(System.Nullable<int> value);
    partial void OnPair2IW3B2Changed();
    #endregion
		
		public S1TilesCoverage()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idrow", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int idrow
		{
			get
			{
				return this._idrow;
			}
			set
			{
				if ((this._idrow != value))
				{
					this.OnidrowChanging(value);
					this.SendPropertyChanging();
					this._idrow = value;
					this.SendPropertyChanged("idrow");
					this.OnidrowChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DegreeTileName", DbType="NVarChar(50)")]
		public string DegreeTileName
		{
			get
			{
				return this._DegreeTileName;
			}
			set
			{
				if ((this._DegreeTileName != value))
				{
					this.OnDegreeTileNameChanging(value);
					this.SendPropertyChanging();
					this._DegreeTileName = value;
					this.SendPropertyChanged("DegreeTileName");
					this.OnDegreeTileNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DTBaseSurvey", DbType="DateTime")]
		public System.Nullable<System.DateTime> DTBaseSurvey
		{
			get
			{
				return this._DTBaseSurvey;
			}
			set
			{
				if ((this._DTBaseSurvey != value))
				{
					this.OnDTBaseSurveyChanging(value);
					this.SendPropertyChanging();
					this._DTBaseSurvey = value;
					this.SendPropertyChanged("DTBaseSurvey");
					this.OnDTBaseSurveyChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idPair1", DbType="Int")]
		public System.Nullable<int> idPair1
		{
			get
			{
				return this._idPair1;
			}
			set
			{
				if ((this._idPair1 != value))
				{
					this.OnidPair1Changing(value);
					this.SendPropertyChanging();
					this._idPair1 = value;
					this.SendPropertyChanged("idPair1");
					this.OnidPair1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idPair2", DbType="Int")]
		public System.Nullable<int> idPair2
		{
			get
			{
				return this._idPair2;
			}
			set
			{
				if ((this._idPair2 != value))
				{
					this.OnidPair2Changing(value);
					this.SendPropertyChanging();
					this._idPair2 = value;
					this.SendPropertyChanged("idPair2");
					this.OnidPair2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW1", DbType="Int")]
		public System.Nullable<int> Pair1IW1
		{
			get
			{
				return this._Pair1IW1;
			}
			set
			{
				if ((this._Pair1IW1 != value))
				{
					this.OnPair1IW1Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW1 = value;
					this.SendPropertyChanged("Pair1IW1");
					this.OnPair1IW1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW1B1", DbType="Int")]
		public System.Nullable<int> Pair1IW1B1
		{
			get
			{
				return this._Pair1IW1B1;
			}
			set
			{
				if ((this._Pair1IW1B1 != value))
				{
					this.OnPair1IW1B1Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW1B1 = value;
					this.SendPropertyChanged("Pair1IW1B1");
					this.OnPair1IW1B1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW1B2", DbType="Int")]
		public System.Nullable<int> Pair1IW1B2
		{
			get
			{
				return this._Pair1IW1B2;
			}
			set
			{
				if ((this._Pair1IW1B2 != value))
				{
					this.OnPair1IW1B2Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW1B2 = value;
					this.SendPropertyChanged("Pair1IW1B2");
					this.OnPair1IW1B2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW2", DbType="Int")]
		public System.Nullable<int> Pair1IW2
		{
			get
			{
				return this._Pair1IW2;
			}
			set
			{
				if ((this._Pair1IW2 != value))
				{
					this.OnPair1IW2Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW2 = value;
					this.SendPropertyChanged("Pair1IW2");
					this.OnPair1IW2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW2B1", DbType="Int")]
		public System.Nullable<int> Pair1IW2B1
		{
			get
			{
				return this._Pair1IW2B1;
			}
			set
			{
				if ((this._Pair1IW2B1 != value))
				{
					this.OnPair1IW2B1Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW2B1 = value;
					this.SendPropertyChanged("Pair1IW2B1");
					this.OnPair1IW2B1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW2B2", DbType="Int")]
		public System.Nullable<int> Pair1IW2B2
		{
			get
			{
				return this._Pair1IW2B2;
			}
			set
			{
				if ((this._Pair1IW2B2 != value))
				{
					this.OnPair1IW2B2Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW2B2 = value;
					this.SendPropertyChanged("Pair1IW2B2");
					this.OnPair1IW2B2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW3", DbType="Int")]
		public System.Nullable<int> Pair1IW3
		{
			get
			{
				return this._Pair1IW3;
			}
			set
			{
				if ((this._Pair1IW3 != value))
				{
					this.OnPair1IW3Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW3 = value;
					this.SendPropertyChanged("Pair1IW3");
					this.OnPair1IW3Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW3B1", DbType="Int")]
		public System.Nullable<int> Pair1IW3B1
		{
			get
			{
				return this._Pair1IW3B1;
			}
			set
			{
				if ((this._Pair1IW3B1 != value))
				{
					this.OnPair1IW3B1Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW3B1 = value;
					this.SendPropertyChanged("Pair1IW3B1");
					this.OnPair1IW3B1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair1IW3B2", DbType="Int")]
		public System.Nullable<int> Pair1IW3B2
		{
			get
			{
				return this._Pair1IW3B2;
			}
			set
			{
				if ((this._Pair1IW3B2 != value))
				{
					this.OnPair1IW3B2Changing(value);
					this.SendPropertyChanging();
					this._Pair1IW3B2 = value;
					this.SendPropertyChanged("Pair1IW3B2");
					this.OnPair1IW3B2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW1", DbType="Int")]
		public System.Nullable<int> Pair2IW1
		{
			get
			{
				return this._Pair2IW1;
			}
			set
			{
				if ((this._Pair2IW1 != value))
				{
					this.OnPair2IW1Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW1 = value;
					this.SendPropertyChanged("Pair2IW1");
					this.OnPair2IW1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW1B1", DbType="Int")]
		public System.Nullable<int> Pair2IW1B1
		{
			get
			{
				return this._Pair2IW1B1;
			}
			set
			{
				if ((this._Pair2IW1B1 != value))
				{
					this.OnPair2IW1B1Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW1B1 = value;
					this.SendPropertyChanged("Pair2IW1B1");
					this.OnPair2IW1B1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW1B2", DbType="Int")]
		public System.Nullable<int> Pair2IW1B2
		{
			get
			{
				return this._Pair2IW1B2;
			}
			set
			{
				if ((this._Pair2IW1B2 != value))
				{
					this.OnPair2IW1B2Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW1B2 = value;
					this.SendPropertyChanged("Pair2IW1B2");
					this.OnPair2IW1B2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW2", DbType="Int")]
		public System.Nullable<int> Pair2IW2
		{
			get
			{
				return this._Pair2IW2;
			}
			set
			{
				if ((this._Pair2IW2 != value))
				{
					this.OnPair2IW2Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW2 = value;
					this.SendPropertyChanged("Pair2IW2");
					this.OnPair2IW2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW2B1", DbType="Int")]
		public System.Nullable<int> Pair2IW2B1
		{
			get
			{
				return this._Pair2IW2B1;
			}
			set
			{
				if ((this._Pair2IW2B1 != value))
				{
					this.OnPair2IW2B1Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW2B1 = value;
					this.SendPropertyChanged("Pair2IW2B1");
					this.OnPair2IW2B1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW2B2", DbType="Int")]
		public System.Nullable<int> Pair2IW2B2
		{
			get
			{
				return this._Pair2IW2B2;
			}
			set
			{
				if ((this._Pair2IW2B2 != value))
				{
					this.OnPair2IW2B2Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW2B2 = value;
					this.SendPropertyChanged("Pair2IW2B2");
					this.OnPair2IW2B2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW3", DbType="Int")]
		public System.Nullable<int> Pair2IW3
		{
			get
			{
				return this._Pair2IW3;
			}
			set
			{
				if ((this._Pair2IW3 != value))
				{
					this.OnPair2IW3Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW3 = value;
					this.SendPropertyChanged("Pair2IW3");
					this.OnPair2IW3Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW3B1", DbType="Int")]
		public System.Nullable<int> Pair2IW3B1
		{
			get
			{
				return this._Pair2IW3B1;
			}
			set
			{
				if ((this._Pair2IW3B1 != value))
				{
					this.OnPair2IW3B1Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW3B1 = value;
					this.SendPropertyChanged("Pair2IW3B1");
					this.OnPair2IW3B1Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pair2IW3B2", DbType="Int")]
		public System.Nullable<int> Pair2IW3B2
		{
			get
			{
				return this._Pair2IW3B2;
			}
			set
			{
				if ((this._Pair2IW3B2 != value))
				{
					this.OnPair2IW3B2Changing(value);
					this.SendPropertyChanging();
					this._Pair2IW3B2 = value;
					this.SendPropertyChanged("Pair2IW3B2");
					this.OnPair2IW3B2Changed();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MilSpace_S1PairCoh")]
	internal partial class S1PairCoherence : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idrow;
		
		private string _idSceneBase;
		
		private string _idScentSlave;
		
		private System.Nullable<double> _fmean;
		
		private System.Nullable<double> _fdeviation;
		
		private System.Nullable<double> _fmin;
		
		private string _fmax;
		
		private System.Nullable<System.DateTime> _dto;
		
		private string _soper;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidrowChanging(int value);
    partial void OnidrowChanged();
    partial void OnidSceneBaseChanging(string value);
    partial void OnidSceneBaseChanged();
    partial void OnidScentSlaveChanging(string value);
    partial void OnidScentSlaveChanged();
    partial void OnfmeanChanging(System.Nullable<double> value);
    partial void OnfmeanChanged();
    partial void OnfdeviationChanging(System.Nullable<double> value);
    partial void OnfdeviationChanged();
    partial void OnfminChanging(System.Nullable<double> value);
    partial void OnfminChanged();
    partial void OnfmaxChanging(string value);
    partial void OnfmaxChanged();
    partial void OndtoChanging(System.Nullable<System.DateTime> value);
    partial void OndtoChanged();
    partial void OnsoperChanging(string value);
    partial void OnsoperChanged();
    #endregion
		
		public S1PairCoherence()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idrow", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int idrow
		{
			get
			{
				return this._idrow;
			}
			set
			{
				if ((this._idrow != value))
				{
					this.OnidrowChanging(value);
					this.SendPropertyChanging();
					this._idrow = value;
					this.SendPropertyChanged("idrow");
					this.OnidrowChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idSceneBase", DbType="NVarChar(128)")]
		public string idSceneBase
		{
			get
			{
				return this._idSceneBase;
			}
			set
			{
				if ((this._idSceneBase != value))
				{
					this.OnidSceneBaseChanging(value);
					this.SendPropertyChanging();
					this._idSceneBase = value;
					this.SendPropertyChanged("idSceneBase");
					this.OnidSceneBaseChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idScentSlave", DbType="NVarChar(128)")]
		public string idScentSlave
		{
			get
			{
				return this._idScentSlave;
			}
			set
			{
				if ((this._idScentSlave != value))
				{
					this.OnidScentSlaveChanging(value);
					this.SendPropertyChanging();
					this._idScentSlave = value;
					this.SendPropertyChanged("idScentSlave");
					this.OnidScentSlaveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fmean", DbType="Float")]
		public System.Nullable<double> fmean
		{
			get
			{
				return this._fmean;
			}
			set
			{
				if ((this._fmean != value))
				{
					this.OnfmeanChanging(value);
					this.SendPropertyChanging();
					this._fmean = value;
					this.SendPropertyChanged("fmean");
					this.OnfmeanChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fdeviation", DbType="Float")]
		public System.Nullable<double> fdeviation
		{
			get
			{
				return this._fdeviation;
			}
			set
			{
				if ((this._fdeviation != value))
				{
					this.OnfdeviationChanging(value);
					this.SendPropertyChanging();
					this._fdeviation = value;
					this.SendPropertyChanged("fdeviation");
					this.OnfdeviationChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fmin", DbType="Float")]
		public System.Nullable<double> fmin
		{
			get
			{
				return this._fmin;
			}
			set
			{
				if ((this._fmin != value))
				{
					this.OnfminChanging(value);
					this.SendPropertyChanging();
					this._fmin = value;
					this.SendPropertyChanged("fmin");
					this.OnfminChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fmax", DbType="NChar(10)")]
		public string fmax
		{
			get
			{
				return this._fmax;
			}
			set
			{
				if ((this._fmax != value))
				{
					this.OnfmaxChanging(value);
					this.SendPropertyChanging();
					this._fmax = value;
					this.SendPropertyChanged("fmax");
					this.OnfmaxChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dto", DbType="DateTime")]
		public System.Nullable<System.DateTime> dto
		{
			get
			{
				return this._dto;
			}
			set
			{
				if ((this._dto != value))
				{
					this.OndtoChanging(value);
					this.SendPropertyChanging();
					this._dto = value;
					this.SendPropertyChanged("dto");
					this.OndtoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_soper", DbType="NVarChar(64)")]
		public string soper
		{
			get
			{
				return this._soper;
			}
			set
			{
				if ((this._soper != value))
				{
					this.OnsoperChanging(value);
					this.SendPropertyChanging();
					this._soper = value;
					this.SendPropertyChanged("soper");
					this.OnsoperChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MilSpace_S1SRC")]
	internal partial class S1Sources : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idrow;
		
		private string _idscene;
		
		private string _extend;
		
		private System.Nullable<System.DateTime> _dttime;
		
		private System.Nullable<int> _nburst;
		
		private System.Nullable<int> _norbit;
		
		private System.Nullable<System.DateTime> _dto;
		
		private string _soper;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidrowChanging(int value);
    partial void OnidrowChanged();
    partial void OnidsceneChanging(string value);
    partial void OnidsceneChanged();
    partial void OnextendChanging(string value);
    partial void OnextendChanged();
    partial void OndttimeChanging(System.Nullable<System.DateTime> value);
    partial void OndttimeChanged();
    partial void OnnburstChanging(System.Nullable<int> value);
    partial void OnnburstChanged();
    partial void OnnorbitChanging(System.Nullable<int> value);
    partial void OnnorbitChanged();
    partial void OndtoChanging(System.Nullable<System.DateTime> value);
    partial void OndtoChanged();
    partial void OnsoperChanging(string value);
    partial void OnsoperChanged();
    #endregion
		
		public S1Sources()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idrow", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int idrow
		{
			get
			{
				return this._idrow;
			}
			set
			{
				if ((this._idrow != value))
				{
					this.OnidrowChanging(value);
					this.SendPropertyChanging();
					this._idrow = value;
					this.SendPropertyChanged("idrow");
					this.OnidrowChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idscene", DbType="NVarChar(128)")]
		public string idscene
		{
			get
			{
				return this._idscene;
			}
			set
			{
				if ((this._idscene != value))
				{
					this.OnidsceneChanging(value);
					this.SendPropertyChanging();
					this._idscene = value;
					this.SendPropertyChanged("idscene");
					this.OnidsceneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_extend", DbType="NVarChar(255)")]
		public string extend
		{
			get
			{
				return this._extend;
			}
			set
			{
				if ((this._extend != value))
				{
					this.OnextendChanging(value);
					this.SendPropertyChanging();
					this._extend = value;
					this.SendPropertyChanged("extend");
					this.OnextendChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dttime", DbType="DateTime")]
		public System.Nullable<System.DateTime> dttime
		{
			get
			{
				return this._dttime;
			}
			set
			{
				if ((this._dttime != value))
				{
					this.OndttimeChanging(value);
					this.SendPropertyChanging();
					this._dttime = value;
					this.SendPropertyChanged("dttime");
					this.OndttimeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_nburst", DbType="Int")]
		public System.Nullable<int> nburst
		{
			get
			{
				return this._nburst;
			}
			set
			{
				if ((this._nburst != value))
				{
					this.OnnburstChanging(value);
					this.SendPropertyChanging();
					this._nburst = value;
					this.SendPropertyChanged("nburst");
					this.OnnburstChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_norbit", DbType="Int")]
		public System.Nullable<int> norbit
		{
			get
			{
				return this._norbit;
			}
			set
			{
				if ((this._norbit != value))
				{
					this.OnnorbitChanging(value);
					this.SendPropertyChanging();
					this._norbit = value;
					this.SendPropertyChanged("norbit");
					this.OnnorbitChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dto", DbType="DateTime")]
		public System.Nullable<System.DateTime> dto
		{
			get
			{
				return this._dto;
			}
			set
			{
				if ((this._dto != value))
				{
					this.OndtoChanging(value);
					this.SendPropertyChanging();
					this._dto = value;
					this.SendPropertyChanged("dto");
					this.OndtoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_soper", DbType="NVarChar(50)")]
		public string soper
		{
			get
			{
				return this._soper;
			}
			set
			{
				if ((this._soper != value))
				{
					this.OnsoperChanging(value);
					this.SendPropertyChanging();
					this._soper = value;
					this.SendPropertyChanged("soper");
					this.OnsoperChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591