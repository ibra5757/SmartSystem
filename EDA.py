import matplotlib as mpl
import matplotlib.pyplot as plt
import matplotlib.dates as mdates
import numpy as np
import pandas as pd
yearly = pd.read_csv('Yearly.csv')
monthly= pd.read_csv('Monthly.csv')
Weekly= pd.read_csv('Weekly.csv')
import matplotlib as mpl
import matplotlib.pyplot as plt
import matplotlib.dates as mdates
plt.style.use('seaborn')
import seaborn as sns
import numpy as np
import pandas as pd
#this wil convert dataset  into weekly monthly and yearlu
def Dailydata(weekly):
    weekly["datum"] = pd.to_datetime(weekly["datum"])
    weekly_summary =weekly.resample('w' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' : 'mean'})
    weekly_summary['datum']=weekly_summary.index  #Weekly 
    return weekly_summary
def Weeklydata(weekly):
    weekly["datum"] = pd.to_datetime(weekly["datum"])
    weekly_summary =weekly.resample('w' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' : 'mean'})
    weekly_summary['datum']=weekly_summary.index   #Weekly 
    return weekly_summary
def Monthlydata(weekly):
    monthly["datum"] = pd.to_datetime(weekly["datum"])
    Monthly_summary =monthly.resample('M' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' : 'mean'})
    weekly_summary['datum']=weekly_summary.index    #monthly
    return Monthly_summary
def Yearlydata(weekly):
    yearly["datum"] = pd.to_datetime(weekly["datum"])
    yearly_summary =yearly.resample('Y' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' :'mean'})
    weekly_summary['datum']=weekly_summary.index    #Yearly
    return yearly_summary


    
def plotgraph(dataset,start_date,end_date):
    dataset['datum'] = pd.to_datetime(dataset['datum'])
    df2 = dataset.query('datum>= @start_date and datum<=@end_date')   #Find Btwn 2 dates
    Date = df2['datum']
    M01AB = df2['M01AB']
    M01AE = df2['M01AE']
    N02BA = df2['N02BA']
    N02BE = df2['N02BE']
    N05B = df2['N05B']
    N05C = df2['N05C']
    R03 = df2['R03']
    R06 = df2['R06']
    plt.rcParams['figure.figsize'] = (14,9)
    plt.rcParams['date.converter'] = 'concise'
    plt.rcParams['date.interval_multiples'] = True
    plt.plot(Date, M01AB, color='tab:purple', label='M01AB')
    plt.plot(Date, M01AE, color='tab:blue', label='M01AE')
    plt.plot(Date, N02BA, color='tab:pink', label='N02BA')
    plt.plot(Date, N02BE, color='tab:red', label='N02BE')
    plt.plot(Date, N05B, color='tab:brown', label='N05BE')
    plt.plot(Date, N05C, color='tab:gray', label='N05C')
    plt.plot(Date, R03, color='tab:cyan', label='R03')
    plt.plot(Date, R06, color='tab:olive', label='R06')
    plt.legend()
    plt.title('Sales ')
    plt.xlabel('Date')
    plt.ylabel('Sales numbers')
    plt.xticks(rotation=45)
    plt.tight_layout()
    plt.show
CleanDataset=pd.read_csv('CleanDataset.csv')
dailydataset=Dailydata(CleanDataset)
plotgraph(dailydataset,'2017-06-01','2017-07-13')
