from flask import Flask,send_file
from flask import jsonify
from flask_cors import CORS
from flask import Response
from sklearn.metrics import mean_squared_error
# import base64
import math
import matplotlib as mpl
import matplotlib.pyplot as plt
import matplotlib.dates as mdates

import numpy as np
from io import StringIO 
import pandas as pd
import base64
import json 
from json import dumps
from flask import request
from sklearn.metrics import mean_squared_error
from sklearn import linear_model
import statsmodels.api as sm
import io
import random
app = Flask(__name__)

CORS(app)
@app.route('/image')
def serve_image():
    
    CleanDataset=pd.read_csv('CleanDataset.csv')
    dailydataset=Dailydata(CleanDataset)
    columns = dailydataset.iloc[:, 0:8]
    sizes = columns.sum(axis=0)
    plt.ion()
    plt.clf()

    plt.figure(figsize=(8, 5))
    sizes.plot.pie()
    buf = io.BytesIO()
    
    # Save the figure to the BytesIO object
    plt.savefig(buf, format='png')

    # Seek to the beginning of the BytesIO object
    buf.seek(0)

    # Create a Flask Response object
    response = Response(buf.getvalue(), mimetype='image/png')
    plt.ioff()
    # Send the Response object to the client
    return response

@app.route('/GetGraphs',methods=['GET', 'POST'])
def dashboardgraphs():
    CleanDataset=pd.read_csv('CleanDataset.csv')
    if request.method == 'POST':
        content = request.json
        startDate=content["startDate"]
        endDate=content["endDate"]
        types=content["type"]
        #3 attributes startdate enddate datetype
        if(types=='Weekly'):
            dailydataset=Dailydata(CleanDataset)
        elif(types=='Monthly'):
            dailydataset=Monthlydata(CleanDataset)
        else:
            dailydataset=Yearlydata(CleanDataset)
        plotgraph(dailydataset,startDate,endDate)
        with open("plot.png", "rb") as img_file:
            my_img = base64.b64encode(img_file.read())
            base64_string = my_img .decode('utf-8')
    else:
        dailydataset=Dailydata(CleanDataset)
        plotgraph(dailydataset,'2017-06-01','2017-07-13')
        with open("plot.png", "rb") as img_file:
            my_img = base64.b64encode(img_file.read())
            base64_string = my_img .decode('utf-8')
    return jsonify(base64_string)

@app.route('/data',methods=['GET', 'POST'])
def modelwithout():
    CleanDataset=pd.read_csv('CleanDataset.csv')
    dailydataset=Dailydata(CleanDataset)
    if request.method == 'POST':
        content = request.json
        data=content["MedName"]

        df= dailydataset
        x = df[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
        y = df[data]  #y true
        # with sklearn
        regr = linear_model.LinearRegression()
        regr.fit(x, y)
        x = sm.add_constant(x) # adding a constant
        model = sm.OLS(y, x).fit()
        predictions=model.predict(x) 
        Y_true =predictions # Y_true = Y (original values)
        Y_pred = y 
        mean_squared_error(Y_true,Y_pred)
        newvar=' , '.join(map(str,regr.coef_))
        new_Error_value=GradientDecent(data,dailydataset)
        my_json_string = json.dumps({'Coefficients': newvar,'Intercept':str(new_Error_value),'meansquareerror':mean_squared_error(Y_true,Y_pred)}, cls=NpEncoder)
        return my_json_string



def Dailydata(weekly):
    weekly["datum"] = pd.to_datetime(weekly["datum"])
    weekly_summary =weekly.resample('w' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' : 'mean'})
    weekly_summary['datum']=weekly_summary.index
    return weekly_summary
def Weeklydata(weekly):
    weekly["datum"] = pd.to_datetime(weekly["datum"])
    weekly_summary =weekly.resample('w' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' : 'mean'})
    weekly_summary['datum']=weekly_summary.index
    return weekly_summary
def Monthlydata(weekly):
    monthly = pd.DataFrame()

    weekly["datum"] = pd.to_datetime(weekly["datum"])
    Monthly_summary =weekly.resample('M' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' : 'mean'})
    Monthly_summary['datum']=Monthly_summary.index
    return Monthly_summary
def Yearlydata(weekly):
    weekly["datum"] = pd.to_datetime(weekly["datum"])
    yearly_summary =weekly.resample('Y' ,on='datum').agg({ 'M01AB' :'sum', 'M01AE' : 'sum', 'N02BA' :'sum', 'N02BE' : 'sum', 'N05B' :'sum', 'N05C' : 'sum', 'R03' :'sum','R06' :'sum', 'Temperature' :'mean', 'Dew Point' :'mean', 'Humidity' :'mean',  'Wind Speed' :'mean','Wind Gust' :'mean', 'Pressure' :'mean', 'Precip.' :'mean',  'Condition_int' :'mean'})
    yearly_summary['datum']=yearly_summary.index
    return yearly_summary
class NpEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, np.integer):
            return int(obj)
        if isinstance(obj, np.floating):
            return float(obj)
        if isinstance(obj, np.ndarray):
            return obj.tolist()
        return json.JSONEncoder.default(self, obj)

    
def plotgraph(dataset,start_date,end_date):
    plt.ion()
    plt.clf()

    dataset['datum'] = pd.to_datetime(dataset['datum'])
    df2 = dataset.query('datum>= @start_date and datum<=@end_date')
    Date = df2['datum']
    M01AB = df2['M01AB']
    M01AE = df2['M01AE']
    N02BA = df2['N02BA']
    N02BE = df2['N02BE']
    N05B = df2['N05B']
    N05C = df2['N05C']
    R03 = df2['R03']
    R06 = df2['R06']
    plt.figure(figsize=(12, 7))
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
    plt.savefig('plot.png')
    plt.ioff()
def initialize(dim):
    b=random.random()
    theta=np.random.rand(dim)
    return b,theta
def predict_Y(b,theta,X):
    return b + np.dot(X,theta)
def get_cost(Y,Y_hat):
    Y_resd=Y-Y_hat
    return np.sum(np.dot(Y_resd.T,Y_resd))/len(Y-Y_resd)
def update_theta(x,y,y_hat,b_0,theta_o,learning_rate):
    db=(np.sum(y_hat-y)*2)/len(y)
    dw=(np.dot((y_hat-y),x)*2)/len(y)
    b_1=b_0-learning_rate*db
    theta_1=theta_o-learning_rate*dw
    return b_1,theta_1
def run_gradient_descent(X,Y,alpha,num_iterations):
    b,theta=initialize(X.shape[1])
    iter_num=0
    gd_iterations_df=pd.DataFrame(columns=['iteration','cost'])
    result_idx=0
    for each_iter in range(num_iterations):
        Y_hat=predict_Y(b,theta,X)
        this_cost=get_cost(Y,Y_hat)
        prev_b=b
        prev_theta=theta
        b,theta=update_theta(X,Y,Y_hat,prev_b,prev_theta,alpha)
        
        if(iter_num%10==0):
            gd_iterations_df.loc[result_idx]=[iter_num,this_cost]
            result_idx=result_idx+1
        iter_num +=1
    print('Final Estimate of b and theta : ',b,theta)
    return gd_iterations_df,b,theta
def eq(b,theta,dataset):
    
#     theta_1=-0.0060332
#     theta_2=-0.17374036
#     theta_3=0.11631381
#     theta_4=0.1387106
#     theta_5=0.35578446
#     theta_6=0.1245894
#     b=0.12571207652211783
    theta_1=theta[0]
    theta_2=theta[1]
    theta_3=theta[2]
    theta_4=theta[3]
    theta_5=theta[4]
    theta_6=theta[5]
#     dataset[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
    h = b + theta_1 * (dataset['Temperature']) + theta_2 * (dataset['Dew Point']) +theta_3 * (dataset['Wind Speed'])+theta_4 * (dataset['Humidity'])+theta_5 * (dataset['Pressure'])+theta_5 *(dataset['Condition_int'])
    return h
def eq2(mednam,Temperature,Dew_Point,Wind_Speed,Humidity,Pressure,Condition_int):
    
    theta_1=-0
    theta_2=-0
    theta_3=0
    theta_4=0
    theta_5=0
    theta_6=0
    b=0
    if(mednam=='M01AE'):
        b=0.10974429805939821
        theta_1=0.16922312
        theta_2=-0.34811197
        theta_3=-0.00793357
        theta_4=0.22804113
        theta_5=0.30024787
        theta_6=0.12085736
    elif(mednam=='M01AB'): 
        b=0.07734082713598746
        theta_1=0.02336378
        theta_2=0.17136099
        theta_3=-0.04728178
        theta_4=0.01117326
        theta_5=0.28352415
        theta_6=0.16554746
    elif(mednam=='N02BA'):
        b=0.13905266435168384
        theta_1=0.00473025
        theta_2=0.06077459
        theta_3=0.14650961
        theta_4=-0.17642258
        theta_5=0.28104615
        theta_6=0.01501625
    elif(mednam=='N02BE'):
        b=0.1209308403000253
        theta_1=0.94807584
        theta_2=0.87551958
        theta_3=0.42229956
        theta_4=0.20677884
        theta_5=0.38927071
        theta_6=0.86043506
    elif(mednam=='NO5B'):
        b=0.0685125218599421  
        theta_1=0.20727194
        theta_2=-0.0648809
        theta_3=0.45236361
        theta_4=0.12265912
        theta_5=0.73448167
        theta_6=0.0424402
    elif(mednam=='N05C'):
        b=0.11903305980878619
        theta_1=0.4187727
        theta_2=-0.26924383
        theta_3=-0.04760725
        theta_4=-0.17619118
        theta_5=0.01723907
        theta_6=0.2487301
    elif(mednam=='R03'):
        b=0.047572109581049626
        theta_1=0.15129935
        theta_2=-0.20417448
        theta_3=-0.25461833
        theta_4=0.15082114
        theta_5=0.44006914
        theta_6=0.0378867
    elif(mednam=='R06'):
        b=0.04165480223291181
        theta_1=0.43382303          
        theta_2=-0.0600344
        theta_3=0.25820715
        theta_4=0.05222996
        theta_5=0.0650751
        theta_6=-0.05424151
    else:
        print("wrong input")

#     dataset[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
#    h = b + theta_1 * (dataset['Temperature']) + theta_2 * (dataset['Dew Point']) +theta_3 * (dataset['Wind Speed'])+theta_4 * (dataset['Humidity'])+theta_5 * (dataset['Pressure'])+theta_5 *(dataset['Condition_int'])
    h = b + theta_1 * np.int(Temperature) + theta_2 * np.int(Dew_Point) +theta_3 * np.float(Wind_Speed)+theta_4 * np.int(Humidity)+theta_5 * np.float(Pressure)+theta_5 *np.int(Condition_int) 
    return h
def GradientDecent(medname,dailydataset):
        x = dailydataset[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
        y = dailydataset[medname]  #y true
        Y=np.array((y-y.mean())/y.std())
        X=x.apply(lambda rec:(rec-rec.mean())/rec.std(),axis=0)
        b,theta=initialize(6)
        print('Bias: ',b,'Weights: ',theta)
        Y_hat=predict_Y(b,theta,X)
        get_cost(Y,Y_hat)
        Y_hat=predict_Y(b,theta,X)
        Y_hat[0:10]
        print("After initialization -Bias: ",b,"theta: ",theta)
        Y_hat=predict_Y(b,theta,X)
        b,theta=update_theta(X,Y,Y_hat,b,theta,0.01)
        print("After first update -Bias: ",b,"theta: ",theta)
        get_cost(Y,Y_hat)
        gd_iterations_df,b,theta=run_gradient_descent(X,Y,alpha=0.001,num_iterations=1000)   
        gd_iterations_df[0:3000]
        Value=eq(b,theta,dailydataset)
        print(Value)
        newvar=' , '.join(map(str,theta))
        Y_true =Value # Y_true = Y (original values)
        Y_pred = y # Y_pred = Y'
        z=mean_squared_error(Y_true,Y_pred)
        z=z/301
        return z
# Temperature, Dew_Point, Wind_Speed, Humidity, Pressure,Condition_int
@app.route('/Applygradientdecent',methods=['GET', 'POST'])    
def PredictValueAccordingTomodel():
    
    if request.method == 'POST':
        
        CleanDataset=pd.read_csv('CleanDataset.csv')
        content = request.json
        medname=content["MedName"]
        types=content["typeselect"]
        Temperature= content['Temperature'] 
        Dew_Point= content['Dew_Point']
        Wind_Speed= content['Wind_Speed'] 
        Humidity= content['Humidity']
        Pressure= content['Pressure']
        Condition_int=content['Condition_int']
        if(types=='Weekly'):
            dailydataset=Dailydata(CleanDataset)
        elif(types=='Monthly'):
            dailydataset=Monthlydata(CleanDataset)
        else:
            dailydataset=Yearlydata(CleanDataset)
        Value=eq2(medname,Temperature, Dew_Point, Wind_Speed, Humidity, Pressure,Condition_int)
        print(Value)
        my_json = json.dumps({'PredictedValues': Value}, cls=NpEncoder)
        return my_json
    



if __name__=="__main__":
    app.run(host='0.0.0.0', port=105)
    