import numpy as np
import json
import pandas as pd
class NpEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, np.integer):
            return int(obj)
        if isinstance(obj, np.floating):
            return float(obj)
        if isinstance(obj, np.ndarray):
            return obj.tolist()
        return json.JSONEncoder.default(self, obj)
import pandas as pd
monthly= pd.read_csv('Monthly.csv')
from sklearn import linear_model
import statsmodels.api as sm
data='N02BE'
x = monthly[['Temperature','Dew Point','Wind Speed','Humidity','Pressure','Condition_int']]
y = monthly[data]  #y true
import json
# with sklearn
regr = linear_model.LinearRegression()
regr.fit(x, y)

print('Intercept: \n', regr.intercept_)
print('Coefficients: \n', regr.coef_)


# with statsmodels
# x = sm.add_constant(x) # adding a constant
 
model = sm.OLS(y, x).fit()
predictions = model.predict(x)  #y predict

print_model = model.summary()
print(print_model)
# results_as_html = print_model.tables[1].as_html()
# pd.read_html(results_as_html, header=0, index_col=0)[0]

print(predictions)

from sklearn.metrics import mean_squared_error

# Given values
Y_true =predictions # Y_true = Y (original values)
# calculated values
Y_pred = y 
mean_squared_error(Y_true,Y_pred)

# my_json_string = json.dumps({'Coefficients': regr.coef_,'Intercept':regr.intercept_,'meansquareerror':mean_squared_error(Y_true,Y_pred)}, cls=NpEncoder)
# newvar=' , '.join(map(str,regr.coef_))
print(mean_squared_error(Y_true,Y_pred))